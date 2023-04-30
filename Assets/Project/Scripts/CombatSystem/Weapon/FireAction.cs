using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(ActiveWeapon))]
    [RequireComponent(typeof(FireEvent))]
    [RequireComponent(typeof(ReloadEvent))]
    #endregion
    public class FireAction : MonoBehaviour
    {
        private ActiveWeapon activeWeapon;
        private FireEvent fireEvent;
        private ReloadEvent reloadEvent;

        private float fireRateTimer = 0f;
        private float prefireTimer = 0f;



        private void Awake()
        {
            activeWeapon = GetComponent<ActiveWeapon>();
            fireEvent = GetComponent<FireEvent>();
            reloadEvent = GetComponent<ReloadEvent>();
        }



        private void OnEnable()
        {
            fireEvent.OnFireAction += FireEvent_OnFireAction;
        }



        private void OnDisable()
        {
            fireEvent.OnFireAction -= FireEvent_OnFireAction;
        }



        private void Update()
        {
            ProcessFireRateTimer();
        }



        private void FireEvent_OnFireAction(FireEvent _sender, OnFireActionArgs _args)
        {
            Fire(_args);
        }



        private void ProcessFireRateTimer()
        {
            fireRateTimer -= Time.deltaTime;
        }



        private void Fire(OnFireActionArgs _args)
        {

            ProcessPrefireTimer(_args);

            if (!_args.isFiring)
                return;

            if (!IsReadyToFire())
                return;

            FireAmmo(_args.angle, _args.weaponAngle, _args.weaponDirectionVector);

            ResetFireRateTimer();
            ResetPrechargeTimer();
        }



        private void ProcessPrefireTimer(OnFireActionArgs _args)
        {
            if (_args.isFiringPreviousFrame)
            {
                prefireTimer -= Time.deltaTime;
                return;
            }

            ResetPrechargeTimer();
        }



        private bool IsReadyToFire()
        {
            Weapon currentWeapon = activeWeapon.GetCurrentWeapon();

            if (!currentWeapon.weaponDetail.isAmmoInfinite && currentWeapon.ammoRemaining <= 0) return false;

            if (!currentWeapon.weaponDetail.isAmmoPerClipInfinite && currentWeapon.ammoPerClipRemaining <= 0)
            {
                reloadEvent.CallOnReloadAction(currentWeapon, 0);
                return false;
            }

            if (fireRateTimer > 0f) return false;

            if (prefireTimer > 0f) return false;

            if (currentWeapon.isReloading) return false;

            return true;
        }



        private void FireAmmo(float _angle, float _weaponAngle, Vector3 _weaponDirectionVector)
        {
            AmmoDetailSO currentAmmoDetail = activeWeapon.GetAmmoDetail();

            if (currentAmmoDetail != null)
                StartCoroutine(FireAmmoCoroutine(currentAmmoDetail, _angle, _weaponAngle, _weaponDirectionVector));
        }



        private IEnumerator FireAmmoCoroutine(AmmoDetailSO _ammoDetail, float _angle, float _weaponAngle, Vector3 _weaponDirectionVector)
        {
            int ammoCounter = 0;
            int ammoPerShot = Random.Range(_ammoDetail.minSpawnCount, _ammoDetail.maxSpawnCount + 1);
            float ammoSpawnInterval = ammoPerShot > 1 ? Random.Range(_ammoDetail.minSpawnInterval, _ammoDetail.maxSpawnInterval) : 0f;

            Weapon currentWeapon = activeWeapon.GetCurrentWeapon();

            while (ammoCounter < ammoPerShot)
            {
                ammoCounter++;

                GameObject ammoPrefab = _ammoDetail.prefabArray[Random.Range(0, _ammoDetail.prefabArray.Length)];
                float ammoSpeed = Random.Range(_ammoDetail.minSpeed, _ammoDetail.maxSpeed);

                IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, activeWeapon.GetShootPosition(), Quaternion.identity);
                ammo.Init(_ammoDetail, ammoSpeed, _angle, _weaponAngle, _weaponDirectionVector);

                yield return new WaitForSeconds(ammoSpawnInterval);
            }

            if (currentWeapon.weaponDetail.isAmmoPerClipInfinite) yield break;

            currentWeapon.ammoPerClipRemaining--;
            currentWeapon.ammoRemaining--;

            fireEvent.CallOnFired(activeWeapon.GetCurrentWeapon());
            PlayFireShootEffect(_angle);
            PlayFireSoundEffect();
        }



        private void ResetFireRateTimer()
        {
            fireRateTimer = activeWeapon.GetCurrentWeapon().weaponDetail.fireRate;
        }



        private void ResetPrechargeTimer()
        {
            prefireTimer = activeWeapon.GetCurrentWeapon().weaponDetail.prefireDelay;
        }



        private void PlayFireShootEffect(float _angle)
        {
            ShootEffectSO currentWeaponShootEffect = activeWeapon.GetCurrentWeapon().weaponDetail.shootEffect;

            if (currentWeaponShootEffect != null && currentWeaponShootEffect.prefab != null)
            {
                ShootEffect shootEffectInstance = (ShootEffect)PoolManager.Instance.ReuseComponent(currentWeaponShootEffect.prefab, activeWeapon.GetEffectPosition(), Quaternion.identity);

                shootEffectInstance.Init(currentWeaponShootEffect, _angle);

                shootEffectInstance.gameObject.SetActive(true);
            }
        }



        private void PlayFireSoundEffect()
        {
            SoundEffectSO currentWeaponFireSoundEffect = activeWeapon.GetCurrentWeapon().weaponDetail.fireSoundEffect;

            if (currentWeaponFireSoundEffect != null)
                SoundEffectManager.Instance.PlaySoundEffect(currentWeaponFireSoundEffect);
        }
    }
}
