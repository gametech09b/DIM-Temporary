using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(ActiveWeapon))]
    [RequireComponent(typeof(FireEvent))]
    #endregion
    public class FireAction : MonoBehaviour {
        private ActiveWeapon activeWeapon;
        private FireEvent fireEvent;

        private float fireRateTimer = 0f;



        private void Awake() {
            activeWeapon = GetComponent<ActiveWeapon>();
            fireEvent = GetComponent<FireEvent>();
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



        private void ProcessFireRateTimer() {
            fireRateTimer -= Time.deltaTime;
        }



        private void FireEvent_OnFireAction(FireEvent sender, OnFireActionArgs args) {
            Fire(args);
        }



        private void Fire(OnFireActionArgs args) {

            if (!args.isFiring) return;

            if (!IsReadyToFire()) return;

            FireAmmo(args.angle, args.weaponAngle, args.weaponDirectionVector);

            ResetFireRateTimer();
        }



        private bool IsReadyToFire() {
            Weapon currentWeapon = activeWeapon.GetCurrentWeapon();


            if (!currentWeapon.weaponDetail.isAmmoInfinite && currentWeapon.ammoRemaining <= 0) return false;

            if (!currentWeapon.weaponDetail.isAmmoPerClipInfinite && currentWeapon.ammoPerClipRemaining <= 0) return false;

            if (fireRateTimer > 0f) return false;

            if (currentWeapon.isReload) return false;

            return true;
        }



        private void FireAmmo(float angle, float weaponAngle, Vector3 weaponDirectionVector) {
            Weapon currentWeapon = activeWeapon.GetCurrentWeapon();
            AmmoDetailSO currentAmmoDetail = activeWeapon.GetAmmoDetail();

            if (currentAmmoDetail != null) {

                GameObject ammoPrefab = currentAmmoDetail.prefabArray[Random.Range(0, currentAmmoDetail.prefabArray.Length)];
                float ammoSpeed = Random.Range(currentAmmoDetail.minSpeed, currentAmmoDetail.maxSpeed);

                IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, activeWeapon.GetShootPosition(), Quaternion.identity);
                ammo.InitAmmo(currentAmmoDetail, ammoSpeed, angle, weaponAngle, weaponDirectionVector);

                if (currentWeapon.weaponDetail.isAmmoPerClipInfinite) return;

                currentWeapon.ammoPerClipRemaining--;
                currentWeapon.ammoRemaining--;

                fireEvent.CallOnFired(activeWeapon.GetCurrentWeapon());
            }
        }



        private void ResetFireRateTimer() {
            fireRateTimer = activeWeapon.GetCurrentWeapon().weaponDetail.fireRate;
        }
    }
}
