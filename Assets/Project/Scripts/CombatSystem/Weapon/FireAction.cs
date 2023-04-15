using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(ActiveWeapon))]
    [RequireComponent(typeof(FireEvent))]
    [RequireComponent(typeof(ReloadEvent))]
    #endregion
    public class FireAction : MonoBehaviour {
        private ActiveWeapon activeWeapon;
        private FireEvent fireEvent;
        private ReloadEvent reloadEvent;

        private float fireRateTimer = 0f;
        private float prefireTimer = 0f;



        private void Awake() {
            activeWeapon = GetComponent<ActiveWeapon>();
            fireEvent = GetComponent<FireEvent>();
            reloadEvent = GetComponent<ReloadEvent>();
        }



        private void OnEnable() {
            fireEvent.OnFireAction += FireEvent_OnFireAction;
        }



        private void OnDisable() {
            fireEvent.OnFireAction -= FireEvent_OnFireAction;
        }



        private void Update() {
            ProcessFireRateTimer();
        }



        private void FireEvent_OnFireAction(FireEvent sender, OnFireActionArgs args) {
            Fire(args);
        }



        private void ProcessFireRateTimer() {
            fireRateTimer -= Time.deltaTime;
        }



        private void Fire(OnFireActionArgs args) {

            ProcessPrefireTimer(args);

            if (!args.isFiring) return;

            if (!IsReadyToFire()) return;

            FireAmmo(args.angle, args.weaponAngle, args.weaponDirectionVector);

            ResetFireRateTimer();
            ResetPrechargeTimer();
        }



        private void ProcessPrefireTimer(OnFireActionArgs args) {
            if (args.isFiringPreviousFrame) {
                prefireTimer -= Time.deltaTime;
                return;
            }

            ResetPrechargeTimer();
        }



        private bool IsReadyToFire() {
            Weapon currentWeapon = activeWeapon.GetCurrentWeapon();

            if (!currentWeapon.weaponDetail.isAmmoInfinite && currentWeapon.ammoRemaining <= 0) return false;

            if (!currentWeapon.weaponDetail.isAmmoPerClipInfinite && currentWeapon.ammoPerClipRemaining <= 0) {
                reloadEvent.CallOnReloadAction(currentWeapon, 0);

                return false;
            }

            if (fireRateTimer > 0f) return false;

            if (prefireTimer > 0f) return false;

            if (currentWeapon.isReloading) return false;

            return true;
        }



        private void FireAmmo(float angle, float weaponAngle, Vector3 weaponDirectionVector) {
            AmmoDetailSO currentAmmoDetail = activeWeapon.GetAmmoDetail();

            if (currentAmmoDetail != null) {
                StartCoroutine(FireAmmoCoroutine(currentAmmoDetail, angle, weaponAngle, weaponDirectionVector));
            }
        }



        private IEnumerator FireAmmoCoroutine(AmmoDetailSO ammoDetail, float angle, float weaponAngle, Vector3 weaponDirectionVector) {
            int ammoCounter = 0;
            int ammoPerShot = Random.Range(ammoDetail.minSpawnCount, ammoDetail.maxSpawnCount + 1);
            float ammoSpawnInterval = ammoPerShot > 1 ? Random.Range(ammoDetail.minSpawnInterval, ammoDetail.maxSpawnInterval) : 0f;

            Weapon currentWeapon = activeWeapon.GetCurrentWeapon();

            while (ammoCounter < ammoPerShot) {
                ammoCounter++;

                GameObject ammoPrefab = ammoDetail.prefabArray[Random.Range(0, ammoDetail.prefabArray.Length)];
                float ammoSpeed = Random.Range(ammoDetail.minSpeed, ammoDetail.maxSpeed);

                IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, activeWeapon.GetShootPosition(), Quaternion.identity);
                ammo.InitAmmo(ammoDetail, ammoSpeed, angle, weaponAngle, weaponDirectionVector);

                yield return new WaitForSeconds(ammoSpawnInterval);
            }

            if (currentWeapon.weaponDetail.isAmmoPerClipInfinite) yield break;

            currentWeapon.ammoPerClipRemaining--;
            currentWeapon.ammoRemaining--;

            fireEvent.CallOnFired(activeWeapon.GetCurrentWeapon());
            PlayFireSoundEffect();
        }



        private void ResetFireRateTimer() {
            fireRateTimer = activeWeapon.GetCurrentWeapon().weaponDetail.fireRate;
        }


        private void ResetPrechargeTimer() {
            prefireTimer = activeWeapon.GetCurrentWeapon().weaponDetail.prefireDelay;
        }



        private void PlayFireSoundEffect() {
            SoundEffectSO currentWeaponFireSoundEffect = activeWeapon.GetCurrentWeapon().weaponDetail.fireSoundEffect;

            if (currentWeaponFireSoundEffect != null) {
                SoundEffectManager.Instance.PlaySoundEffect(currentWeaponFireSoundEffect);
            }
        }
    }
}
