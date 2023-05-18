using System.Collections;
using TMPro;
using UnityEngine;

using DIM.AudioSystem;
using DIM.CombatSystem;
using DIM.Effect;
using DIM.Environment;
using DIM.PlayerSystem;

namespace DIM.ChestSystem {
    #region Requirement Components
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]

    [RequireComponent(typeof(MaterializeEffect))]
    #endregion
    public class Chest : MonoBehaviour, IUseable {
        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private MaterializeEffect materializeEffect;

        [ColorUsage(false, true)][SerializeField] private Color materializeColor;

        [SerializeField] private float materializeDuration = 3f;
        [SerializeField] private Transform itemSpawnPointTransform;

        private int ammoPercentReward;
        private int healthPercentReward;
        private WeaponDetailSO weaponDetailReward;

        private bool isEnabled = false;
        private ChestState chestState = ChestState.CLOSED;
        private GameObject chestItemGameObject;
        private ChestItem chestItem;
        private TextMeshPro messageTextMP;

        // ===================================================================

        private void Awake() {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            materializeEffect = GetComponent<MaterializeEffect>();
        }



        public void Init(bool _isNeedMaterialize, int _ammoPercentReward, int _healthPercentReward, WeaponDetailSO _weaponDetailReward) {
            isEnabled = true;

            ammoPercentReward = _ammoPercentReward;
            healthPercentReward = _healthPercentReward;
            weaponDetailReward = _weaponDetailReward;

            if (_isNeedMaterialize) {
                StartCoroutine(MaterializeCoroutine());
            } else {
                EnableChest();
            }
        }



        private IEnumerator MaterializeCoroutine() {
            SpriteRenderer[] spriteRendererArray = new SpriteRenderer[] { spriteRenderer };

            yield return StartCoroutine(materializeEffect.MaterializeCoroutine(GameResources.Instance.MaterializeShader, materializeColor, materializeDuration, spriteRendererArray, GameResources.Instance.LitMaterial));

            EnableChest();
        }



        private void EnableChest() {
            isEnabled = true;
        }



        public void Use() {
            if (!isEnabled)
                return;

            switch (chestState) {
                case ChestState.CLOSED:
                    OpenChest();
                    break;

                case ChestState.AMMO_ITEM:
                    RewardAmmoItem();
                    break;

                case ChestState.HEALTH_ITEM:
                    RewardHealthItem();
                    break;

                case ChestState.WEAPON_ITEM:
                    RewardWeaponItem();
                    break;

                case ChestState.EMPTY:
                    return;

                default:
                    return;
            }
        }



        private void OpenChest() {
            animator.SetBool(Settings.Use, true);

            SoundEffectManager.Instance.PlaySoundEffect(AudioResources.Instance.ChestOpenSoundEffect);

            if (weaponDetailReward != null) {
                if (GameManager.Instance.GetCurrentPlayer().IsWeaponInList(weaponDetailReward))
                    weaponDetailReward = null;
            }

            UpdateChestState();
        }



        private void UpdateChestState() {
            if (ammoPercentReward > 0) {
                chestState = ChestState.AMMO_ITEM;
                InstantiateAmmoItem();

            } else if (healthPercentReward > 0) {
                chestState = ChestState.HEALTH_ITEM;
                InstantiateHealthItem();

            } else if (weaponDetailReward != null) {
                chestState = ChestState.WEAPON_ITEM;
                InstantiateWeaponItem();

            } else {
                chestState = ChestState.EMPTY;
            }
        }



        private void InstantiateItem() {
            chestItemGameObject = Instantiate(UIResources.Instance.ChestItemPrefab, this.transform);
            chestItem = chestItemGameObject.GetComponent<ChestItem>();
        }



        private void InstantiateAmmoItem() {
            InstantiateItem();

            string message = $"{ammoPercentReward.ToString()}%";

            chestItem.Init(UIResources.Instance.BulletIconSprite, message, itemSpawnPointTransform.position, materializeColor);
        }



        private void RewardAmmoItem() {
            if (chestItem == null
            || !chestItem.isMaterialized)
                return;

            Player currentPlayer = GameManager.Instance.GetCurrentPlayer();

            currentPlayer.reloadEvent.CallOnReloadAction(currentPlayer.activeWeapon.GetCurrentWeapon(), ammoPercentReward);

            SoundEffectManager.Instance.PlaySoundEffect(AudioResources.Instance.ChestAmmoPickupSoundEffect);

            ammoPercentReward = 0;

            Destroy(chestItemGameObject);

            UpdateChestState();
        }



        private void InstantiateHealthItem() {
            InstantiateItem();

            string message = $"{healthPercentReward.ToString()}%";

            chestItem.Init(UIResources.Instance.HeartIconSprite, message, itemSpawnPointTransform.position, materializeColor);
        }



        private void RewardHealthItem() {
            if (chestItem == null
            || !chestItem.isMaterialized)
                return;

            Player currentPlayer = GameManager.Instance.GetCurrentPlayer();


            currentPlayer.health.AddHealth(healthPercentReward);

            SoundEffectManager.Instance.PlaySoundEffect(AudioResources.Instance.ChestHealthPickupSoundEffect);

            healthPercentReward = 0;

            Destroy(chestItemGameObject);

            UpdateChestState();
        }



        private void InstantiateWeaponItem() {
            InstantiateItem();

            string message = weaponDetailReward.weaponName;

            chestItem.Init(weaponDetailReward.sprite, message, itemSpawnPointTransform.position, materializeColor);
        }



        private void RewardWeaponItem() {
            if (chestItem == null
            || !chestItem.isMaterialized)
                return;

            Player currentPlayer = GameManager.Instance.GetCurrentPlayer();

            if (!currentPlayer.IsWeaponInList(weaponDetailReward)) {
                currentPlayer.AddWeapon(weaponDetailReward);

                SoundEffectManager.Instance.PlaySoundEffect(AudioResources.Instance.ChestWeaponPickupSoundEffect);
            } else {
                StartCoroutine(ShowMessageCoroutine("You already have this weapon", 5f));
            }

            weaponDetailReward = null;

            Destroy(chestItemGameObject);

            UpdateChestState();
        }



        private IEnumerator ShowMessageCoroutine(string _message, float _duration) {
            messageTextMP.text = _message;

            yield return new WaitForSeconds(_duration);

            messageTextMP.text = "";
        }
    }
}
