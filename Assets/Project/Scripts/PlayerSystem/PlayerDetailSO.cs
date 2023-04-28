using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [CreateAssetMenu(fileName = "PlayerDetail_", menuName = "Scriptable Objects/Player/Player Detail")]
    public class PlayerDetailSO : ScriptableObject
    {
        [Space(10)]
        [Header("Player Base Detail")]

        [Tooltip("Player Character Name")]
        public string characterName;

        [Tooltip("Prefab gameobject for the player character")]
        public GameObject characterPrefab;

        [Tooltip("Player runtime animator controller")]
        public RuntimeAnimatorController runtimeAnimatorController;



        [Space(10)]
        [Header("Player Stats")]

        [Tooltip("Player starting health amount")]
        public int startingHealthAmount;



        [Space(10)]
        [Header("Others")]

        [Tooltip("Player icon sprite for minimap")]
        public Sprite minimapIconSprite;

        [Tooltip("Player hand sprite")]
        public Sprite handSprite;



        [Space(10)]
        [Header("Player Weapon")]


        [Tooltip("Player initial weapon")]
        public WeaponDetailSO initialWeapon;

        [Tooltip("Player list of initial weapons")]
        public List<WeaponDetailSO> initialWeaponsList;



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckEmptyString(this, nameof(characterName), characterName);
            HelperUtilities.CheckNullValue(this, nameof(characterPrefab), characterPrefab);
            HelperUtilities.CheckNullValue(this, nameof(runtimeAnimatorController), runtimeAnimatorController);
            HelperUtilities.CheckPositiveValue(this, nameof(startingHealthAmount), startingHealthAmount);
            HelperUtilities.CheckNullValue(this, nameof(minimapIconSprite), minimapIconSprite);
            HelperUtilities.CheckNullValue(this, nameof(handSprite), handSprite);

            HelperUtilities.CheckNullValue(this, nameof(initialWeapon), initialWeapon);
            HelperUtilities.CheckEnumerableValue(this, nameof(initialWeaponsList), initialWeaponsList);
        }
#endif
        #endregion
    }
}
