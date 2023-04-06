using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(ActiveWeaponEvent))]
    [RequireComponent(typeof(ReloadEvent))]
    #endregion
    public class ReloadAction : MonoBehaviour {
        private ActiveWeaponEvent activeWeaponEvent;
        private ReloadEvent reloadEvent;

        private Coroutine reloadCoroutine;



        private void Awake() {
            activeWeaponEvent = GetComponent<ActiveWeaponEvent>();
            reloadEvent = GetComponent<ReloadEvent>();
        }



        private void OnEnable() {
            activeWeaponEvent.OnSetActiveWeapon += ActiveWeaponEvent_OnSetActiveWeapon;
            reloadEvent.OnReloadAction += ReloadEvent_OnReloadAction;
        }



        private void OnDisable() {
            activeWeaponEvent.OnSetActiveWeapon -= ActiveWeaponEvent_OnSetActiveWeapon;
            reloadEvent.OnReloadAction -= ReloadEvent_OnReloadAction;
        }



        private void ActiveWeaponEvent_OnSetActiveWeapon(ActiveWeaponEvent sender, OnSetActiveWeaponArgs args) {
            if (args.weapon.isReloading) {
                if (reloadCoroutine != null) {
                    StopCoroutine(reloadCoroutine);
                }

                reloadCoroutine = StartCoroutine(ReloadCoroutine(args.weapon, 0));
            }
        }



        private void ReloadEvent_OnReloadAction(ReloadEvent sender, OnReloadActionArgs args) {
            Reload(args);
        }



        private void Reload(OnReloadActionArgs args) {
            if (reloadCoroutine != null) {
                StopCoroutine(reloadCoroutine);
            }

            reloadCoroutine = StartCoroutine(ReloadCoroutine(args.weapon, args.reloadAmmoPercent));
        }



        private IEnumerator ReloadCoroutine(Weapon weapon, int reloadAmmoPercent) {
            weapon.isReloading = true;

            while (weapon.reloadTimer < weapon.weaponDetail.reloadTime) {
                weapon.reloadTimer += Time.deltaTime;
                yield return null;
            }

            if (reloadAmmoPercent != 0) {
                int reloadAmmoCount = Mathf.RoundToInt((weapon.weaponDetail.ammoCapacity * reloadAmmoPercent) / 100);
                int totalAmmo = weapon.ammoRemaining + reloadAmmoCount;

                if (totalAmmo > weapon.weaponDetail.ammoCapacity) {
                    weapon.ammoRemaining = weapon.weaponDetail.ammoCapacity;
                } else {
                    weapon.ammoRemaining = totalAmmo;
                }
            }

            if (weapon.weaponDetail.isAmmoInfinite) {
                weapon.ammoPerClipRemaining = weapon.weaponDetail.ammoPerClipCapacity;
            } else if (weapon.ammoRemaining >= weapon.weaponDetail.ammoPerClipCapacity) {
                weapon.ammoPerClipRemaining = weapon.weaponDetail.ammoPerClipCapacity;
            } else {
                weapon.ammoPerClipRemaining = weapon.ammoRemaining;
            }

            weapon.reloadTimer = 0;
            weapon.isReloading = false;

            reloadEvent.CallOnReloaded(weapon);
        }
    }
}

