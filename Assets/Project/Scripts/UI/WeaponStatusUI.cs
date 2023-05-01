using System.Threading;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DungeonGunner
{
    public class WeaponStatusUI : MonoBehaviour
    {

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



        private void Awake()
        {
            player = GameManager.Instance.GetCurrentPlayer();
        }



        private void OnEnable()
        {
            player.activeWeaponEvent.OnSetActiveWeapon += ActiveWeapon_OnSetActiveWeapon;
            player.fireEvent.OnFired += FireEvent_OnFired;
            player.reloadEvent.OnReloadAction += ReloadEvent_OnReloadAction;
            player.reloadEvent.OnReloaded += ReloadEvent_OnReloaded;
        }



        private void OnDisable()
        {
            player.activeWeaponEvent.OnSetActiveWeapon -= ActiveWeapon_OnSetActiveWeapon;
            player.fireEvent.OnFired -= FireEvent_OnFired;
            player.reloadEvent.OnReloadAction -= ReloadEvent_OnReloadAction;
            player.reloadEvent.OnReloaded -= ReloadEvent_OnReloaded;
        }



        private void Start()
        {
            SetActiveWeaponUI(player.activeWeapon.GetCurrentWeapon());
        }



        private void ActiveWeapon_OnSetActiveWeapon(ActiveWeaponEvent _sender, OnSetActiveWeaponArgs _args)
        {
            SetActiveWeaponUI(_args.weapon);
        }



        private void FireEvent_OnFired(FireEvent _sender, OnFiredArgs _args)
        {
            FiredUI(_args.weapon);
        }



        private void ReloadEvent_OnReloadAction(ReloadEvent _sender, OnReloadActionArgs _args)
        {
            ReloadActionUI(_args.weapon);
        }



        private void ReloadEvent_OnReloaded(ReloadEvent _sender, OnReloadedArgs _args)
        {
            ReloadedUI(_args.weapon);
        }



        private void SetActiveWeaponUI(Weapon _weapon)
        {
            UpdateActiveWeaponImage(_weapon.weaponDetail);
            UpdateActiveWeaponName(_weapon);
            UpdateAmmoText(_weapon);
            UpdateAmmoIconList(_weapon);

            Debug.Log($"Set Active Weapon UI: {_weapon.isReloading}");

            if (_weapon.isReloading)
                ReloadActionUI(_weapon);
            else
                ResetStatusBar();

            UpdateReloadText(_weapon);
        }



        private void FiredUI(Weapon _weapon)
        {
            UpdateAmmoIconList(_weapon);
            UpdateAmmoText(_weapon);
            UpdateReloadText(_weapon);
        }



        private void ReloadedUI(Weapon _weapon)
        {
            if (player.activeWeapon.GetCurrentWeapon() == _weapon)
            {
                UpdateAmmoIconList(_weapon);
                UpdateAmmoText(_weapon);
                UpdateReloadText(_weapon);

                ResetStatusBar();
            }
        }



        private void UpdateActiveWeaponImage(WeaponDetailSO _weaponDetail)
        {
            weaponImage.sprite = _weaponDetail.sprite;
        }



        private void UpdateActiveWeaponName(Weapon _weapon)
        {
            weaponNameText.text = $"({_weapon.indexOnList}) {_weapon.weaponDetail.weaponName.ToUpper()}";
        }



        private void UpdateAmmoIconList(Weapon _weapon)
        {
            ClearAmmoIconList();

            for (int i = 0; i < _weapon.ammoPerClipRemaining; i++)
            {
                GameObject ammoIcon = Instantiate(UIResources.Instance.ammoIconPrefab, ammoIconListParentTransform);
                RectTransform ammoIconRectTransform = ammoIcon.GetComponent<RectTransform>();
                ammoIconRectTransform.anchoredPosition = new Vector2(0, Settings.UIAmmoIconSpacing * i);

                ammoIconList.Add(ammoIcon);
            }
        }



        private void ClearAmmoIconList()
        {
            foreach (GameObject ammoIcon in ammoIconList)
            {
                Destroy(ammoIcon);
            }
            ammoIconList.Clear();
        }



        private void UpdateAmmoText(Weapon _weapon)
        {
            if (_weapon.weaponDetail.isAmmoInfinite)
            {
                ammoRemainingText.text = "INFINITE AMMO";
                return;
            }

            ammoRemainingText.text = $"{_weapon.ammoRemaining}/{_weapon.weaponDetail.ammoCapacity}";
        }



        private void ReloadActionUI(Weapon _weapon)
        {
            if (_weapon.weaponDetail.isAmmoPerClipInfinite)
                return;

            StopReloadingCoroutine();
            UpdateReloadText(_weapon);

            reloadingCoroutine = StartCoroutine(ReloadingCoroutine(_weapon));
        }



        private IEnumerator ReloadingCoroutine(Weapon _weapon)
        {
            statusBarValueImage.color = Color.red;

            while (_weapon.isReloading)
            {
                float barFill = _weapon.reloadTimer / _weapon.weaponDetail.reloadTime;
                statusBarValueTransform.transform.localScale = new Vector3(barFill, 1, 1);

                yield return null;
            }
        }



        private void ResetStatusBar()
        {
            StopReloadingCoroutine();

            statusBarValueImage.color = Color.green;
            statusBarValueTransform.transform.localScale = new Vector3(1f, 1f, 1f);
        }



        private void StopReloadingCoroutine()
        {
            if (reloadingCoroutine != null)
            {
                StopCoroutine(reloadingCoroutine);
            }
        }



        private void UpdateReloadText(Weapon _weapon)
        {
            if (!_weapon.weaponDetail.isAmmoPerClipInfinite
            && (_weapon.ammoPerClipRemaining <= 0 || _weapon.isReloading))
            {
                statusBarValueImage.color = Color.red;

                StopBlinkingReloadingTextCoroutine();

                blinkingReloadingTextCoroutine = StartCoroutine(BlinkingReloadingTextCoroutine());
            }
            else
            {
                StopBlinkingReloadingText();
            }
        }



        private IEnumerator BlinkingReloadingTextCoroutine()
        {
            while (true)
            {
                reloadText.text = "RELOAD";
                yield return new WaitForSeconds(0.1f);
                reloadText.text = "";
                yield return new WaitForSeconds(0.1f);
            }
        }



        private void StopBlinkingReloadingTextCoroutine()
        {
            if (blinkingReloadingTextCoroutine != null)
            {
                StopCoroutine(blinkingReloadingTextCoroutine);
            }
        }



        private void StopBlinkingReloadingText()
        {
            StopBlinkingReloadingTextCoroutine();
            reloadText.text = "";
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckNullValue(this, nameof(reloadText), reloadText);
            HelperUtilities.CheckNullValue(this, nameof(statusBarValueTransform), statusBarValueTransform);
            HelperUtilities.CheckNullValue(this, nameof(statusBarValueImage), statusBarValueImage);
            HelperUtilities.CheckNullValue(this, nameof(weaponImage), weaponImage);
            HelperUtilities.CheckNullValue(this, nameof(weaponNameText), weaponNameText);
            HelperUtilities.CheckNullValue(this, nameof(ammoRemainingText), ammoRemainingText);
            HelperUtilities.CheckNullValue(this, nameof(ammoIconListParentTransform), ammoIconListParentTransform);
        }
#endif
        #endregion
    }
}
