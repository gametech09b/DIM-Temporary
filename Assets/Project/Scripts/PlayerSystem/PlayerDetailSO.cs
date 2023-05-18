using System.Collections.Generic;
using UnityEngine;

using DIM.CombatSystem;

namespace DIM.PlayerSystem {
    [CreateAssetMenu(fileName = "PlayerDetail_", menuName = "Scriptable Objects/Player/Player Detail")]
    public class PlayerDetailSO : ScriptableObject {
        [Space(10)]
        [Header("Player Base Detail")]

        public string characterName;
        public GameObject characterPrefab;
        public RuntimeAnimatorController runtimeAnimatorController;


        [Space(10)]
        [Header("Player Stats")]

        public int startingHealthAmount;
        public bool isImmuneAfterHit = false;
        public float immuneDuration;


        [Space(10)]
        [Header("Others")]

        public Sprite minimapIconSprite;
        public Sprite handSprite;


        [Space(10)]
        [Header("Player Weapon")]

        public WeaponDetailSO initialWeapon;
        public List<WeaponDetailSO> initialWeaponsList;



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckEmptyString(this, nameof(characterName), characterName);
            HelperUtilities.CheckNullValue(this, nameof(characterPrefab), characterPrefab);
            HelperUtilities.CheckNullValue(this, nameof(runtimeAnimatorController), runtimeAnimatorController);

            HelperUtilities.CheckPositiveValue(this, nameof(startingHealthAmount), startingHealthAmount);
            if (isImmuneAfterHit)
                HelperUtilities.CheckPositiveValue(this, nameof(immuneDuration), immuneDuration);

            HelperUtilities.CheckNullValue(this, nameof(minimapIconSprite), minimapIconSprite);
            HelperUtilities.CheckNullValue(this, nameof(handSprite), handSprite);

            HelperUtilities.CheckNullValue(this, nameof(initialWeapon), initialWeapon);
            HelperUtilities.CheckEnumerableValue(this, nameof(initialWeaponsList), initialWeaponsList);
        }
#endif
        #endregion
    }
}
