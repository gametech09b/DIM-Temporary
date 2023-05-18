using System.Collections;
using UnityEngine;

using DIM.AudioSystem;

namespace DIM.CombatSystem {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(ActiveWeaponEvent))]
    [RequireComponent(typeof(ReloadEvent))]
    #endregion
    public class ReloadAction : MonoBehaviour {
        private ActiveWeaponEvent activeWeaponEvent;
        private ReloadEvent reloadEvent;

        private Coroutine reloadCoroutine;

        // ===================================================================

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



        private void ActiveWeaponEvent_OnSetActiveWeapon(ActiveWeaponEvent _sender, OnSetActiveWeaponArgs _args) {
            if (_args.weapon.isReloading) {
                if (reloadCoroutine != null)
                    StopCoroutine(reloadCoroutine);

                reloadCoroutine = StartCoroutine(ReloadCoroutine(_args.weapon, 0));
            }
        }



        private void ReloadEvent_OnReloadAction(ReloadEvent _sender, OnReloadActionArgs _args) {
            Reload(_args);
        }



        private void Reload(OnReloadActionArgs _args) {
            if (reloadCoroutine != null)
                StopCoroutine(reloadCoroutine);

            reloadCoroutine = StartCoroutine(ReloadCoroutine(_args.weapon, _args.reloadAmmoPercent));
        }



        private IEnumerator ReloadCoroutine(Weapon _weapon, int _reloadAmmoPercent) {
            SoundEffectSO currentReloadSounfEffect = _weapon.weaponDetail.reloadSoundEffect;
            if (!_weapon.isReloading && currentReloadSounfEffect != null)
                SoundEffectManager.Instance.PlaySoundEffect(currentReloadSounfEffect);

            _weapon.isReloading = true;

            while (_weapon.reloadTimer < _weapon.weaponDetail.reloadTime) {
                _weapon.reloadTimer += Time.deltaTime;
                yield return null;
            }

            if (_reloadAmmoPercent != 0) {
                int reloadAmmoCount = Mathf.RoundToInt((_weapon.weaponDetail.ammoCapacity * _reloadAmmoPercent) / 100);
                int totalAmmo = _weapon.ammoRemaining + reloadAmmoCount;

                if (totalAmmo > _weapon.weaponDetail.ammoCapacity)
                    _weapon.ammoRemaining = _weapon.weaponDetail.ammoCapacity;
                else
                    _weapon.ammoRemaining = totalAmmo;
            }

            if (_weapon.weaponDetail.isAmmoInfinite)
                _weapon.ammoPerClipRemaining = _weapon.weaponDetail.ammoPerClipCapacity;
            else if (_weapon.ammoRemaining >= _weapon.weaponDetail.ammoPerClipCapacity)
                _weapon.ammoPerClipRemaining = _weapon.weaponDetail.ammoPerClipCapacity;
            else
                _weapon.ammoPerClipRemaining = _weapon.ammoRemaining;

            _weapon.reloadTimer = 0;
            _weapon.isReloading = false;

            reloadEvent.CallOnReloaded(_weapon);
        }
    }
}

