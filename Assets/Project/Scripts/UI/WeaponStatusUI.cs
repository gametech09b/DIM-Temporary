using System.Threading;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonGunner {
    public class WeaponStatusUI : MonoBehaviour {

        [Space(10)]
        [Header("UI Object References")]


        [SerializeField] private Transform ammoIconListParentTransform;

        [SerializeField] private Image weaponImage;

        [SerializeField] private Transform statusBarValueTransform;
        [SerializeField] private Image statusBarValueImage;

        [SerializeField] private TextMeshProUGUI weaponNameText;

        [SerializeField] private TextMeshProUGUI reloadText;
        [SerializeField] private TextMeshProUGUI ammoRemainingText;



        private Player player;
        private List<GameObject> ammoIconList = new List<GameObject>();
        private Coroutine reloadingCoroutine;
        private Coroutine blinkingReloadingTextCoroutine;



        private void Awake() {
            player = GameManager.Instance.GetCurrentPlayer();
        }



        private void OnEnable() {
            player.activeWeaponEvent.OnSetActiveWeapon += ActiveWeapon_OnSetActiveWeapon;
            player.fireEvent.OnFired += FireEvent_OnFired;
            player.reloadEvent.OnReloadAction += ReloadEvent_OnReloadAction;
            player.reloadEvent.OnReloaded += ReloadEvent_OnReloaded;
        }



        private void OnDisable() {
            player.activeWeaponEvent.OnSetActiveWeapon -= ActiveWeapon_OnSetActiveWeapon;
            player.fireEvent.OnFired -= FireEvent_OnFired;
            player.reloadEvent.OnReloadAction -= ReloadEvent_OnReloadAction;
            player.reloadEvent.OnReloaded -= ReloadEvent_OnReloaded;
        }



        private void Start() {
            SetActiveWeaponUI(player.activeWeapon.GetCurrentWeapon());
        }



        private void ActiveWeapon_OnSetActiveWeapon(ActiveWeaponEvent sender, OnSetActiveWeaponArgs args) {
            SetActiveWeaponUI(args.weapon);
        }



        private void FireEvent_OnFired(FireEvent sender, OnFiredArgs args) {
            FiredUI(args.weapon);
        }



        private void ReloadEvent_OnReloadAction(ReloadEvent sender, OnReloadActionArgs args) {
            ReloadActionUI(args.weapon);
        }



        private void ReloadEvent_OnReloaded(ReloadEvent sender, OnReloadedArgs args) {
            ReloadedUI(args.weapon);
        }



        private void SetActiveWeaponUI(Weapon weapon) {
            UpdateActiveWeaponImage(weapon.weaponDetail);
            UpdateActiveWeaponName(weapon);
            UpdateAmmoIconList(weapon);
            UpdateAmmoText(weapon);

            if (weapon.isReloading) {
                ReloadActionUI(weapon);
            } else {
                ResetStatusBar();
            }

            UpdateReloadText(weapon);
        }



        private void FiredUI(Weapon weapon) {
            UpdateAmmoIconList(weapon);
            UpdateAmmoText(weapon);
            UpdateReloadText(weapon);
        }



        private void ReloadedUI(Weapon weapon) {
            if (player.activeWeapon.GetCurrentWeapon() == weapon) {
                UpdateAmmoIconList(weapon);
                UpdateAmmoText(weapon);
                UpdateReloadText(weapon);

                ResetStatusBar();
            }
        }



        private void UpdateActiveWeaponImage(WeaponDetailSO weaponDetail) {
            weaponImage.sprite = weaponDetail.sprite;
        }



        private void UpdateActiveWeaponName(Weapon weapon) {
            weaponNameText.text = $"({weapon.indexOnList}) {weapon.weaponDetail.weaponName.ToUpper()}";
        }



        private void UpdateAmmoIconList(Weapon weapon) {
            ClearAmmoIconList();

            for (int i = 0; i < weapon.ammoPerClipRemaining; i++) {
                GameObject ammoIcon = Instantiate(UIResources.Instance.ammoIconPrefab, ammoIconListParentTransform);
                RectTransform ammoIconRectTransform = ammoIcon.GetComponent<RectTransform>();
                ammoIconRectTransform.anchoredPosition = new Vector2(0, Settings.AmmoIconSpacing * i);

                ammoIconList.Add(ammoIcon);
            }
        }



        private void ClearAmmoIconList() {
            foreach (GameObject ammoIcon in ammoIconList) {
                Destroy(ammoIcon);
            }
            ammoIconList.Clear();
        }



        private void UpdateAmmoText(Weapon weapon) {
            if (weapon.weaponDetail.isAmmoInfinite) {
                ammoRemainingText.text = "INFINITE AMMO";
                return;
            }

            ammoRemainingText.text = $"{weapon.ammoRemaining}/{weapon.weaponDetail.ammoCapacity}";
        }



        private void ReloadActionUI(Weapon weapon) {
            if (weapon.weaponDetail.isAmmoPerClipInfinite) return;

            StopReloadingCoroutine();
            UpdateReloadText(weapon);

            reloadingCoroutine = StartCoroutine(ReloadingCoroutine(weapon));
        }



        private IEnumerator ReloadingCoroutine(Weapon weapon) {
            statusBarValueImage.color = Settings.ReloadProgressColor;

            while (weapon.isReloading) {
                float barFill = weapon.reloadTimer / weapon.weaponDetail.reloadTime;
                statusBarValueTransform.transform.localScale = new Vector3(barFill, 1, 1);

                yield return null;
            }
        }



        private void ResetStatusBar() {
            StopReloadingCoroutine();

            statusBarValueImage.color = Settings.ReloadDoneColor;
            statusBarValueImage.transform.localScale = new Vector3(1, 1, 1);
        }



        private void StopReloadingCoroutine() {
            if (reloadingCoroutine != null) {
                StopCoroutine(reloadingCoroutine);
            }
        }



        private void UpdateReloadText(Weapon weapon) {
            if (
                !weapon.weaponDetail.isAmmoPerClipInfinite &&
                (weapon.ammoPerClipRemaining <= 0 || weapon.isReloading)
            ) {
                reloadText.color = Settings.ReloadProgressColor;

                StopBlinkingReloadingTextCoroutine();

                blinkingReloadingTextCoroutine = StartCoroutine(BlinkingReloadingTextCoroutine());
            } else {
                StopBlinkingReloadingText();
            }
        }



        private IEnumerator BlinkingReloadingTextCoroutine() {
            while (true) {
                reloadText.text = "RELOAD";
                yield return new WaitForSeconds(0.1f);
                reloadText.text = "";
                yield return new WaitForSeconds(0.1f);
            }
        }



        private void StopBlinkingReloadingTextCoroutine() {
            if (blinkingReloadingTextCoroutine != null) {
                StopCoroutine(blinkingReloadingTextCoroutine);
            }
        }



        private void StopBlinkingReloadingText() {
            StopBlinkingReloadingTextCoroutine();
            reloadText.text = "";
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.ValidateCheckNullValue(this, nameof(reloadText), reloadText);
            HelperUtilities.ValidateCheckNullValue(this, nameof(statusBarValueTransform), statusBarValueTransform);
            HelperUtilities.ValidateCheckNullValue(this, nameof(statusBarValueImage), statusBarValueImage);
            HelperUtilities.ValidateCheckNullValue(this, nameof(weaponImage), weaponImage);
            HelperUtilities.ValidateCheckNullValue(this, nameof(weaponNameText), weaponNameText);
            HelperUtilities.ValidateCheckNullValue(this, nameof(ammoRemainingText), ammoRemainingText);
            HelperUtilities.ValidateCheckNullValue(this, nameof(ammoIconListParentTransform), ammoIconListParentTransform);
        }
#endif
        #endregion
    }
}
