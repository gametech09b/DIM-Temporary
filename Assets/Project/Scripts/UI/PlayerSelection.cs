using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class PlayerSelection : MonoBehaviour {
        public SpriteRenderer handSpriteRenderer;
        public SpriteRenderer handNoWeaponSpriteRenderer;
        public SpriteRenderer weaponSpriteRenderer;
        public Animator animator;



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckNullValue(this, nameof(handSpriteRenderer), handSpriteRenderer);
            HelperUtilities.CheckNullValue(this, nameof(handNoWeaponSpriteRenderer), handNoWeaponSpriteRenderer);
            HelperUtilities.CheckNullValue(this, nameof(weaponSpriteRenderer), weaponSpriteRenderer);
            HelperUtilities.CheckNullValue(this, nameof(animator), animator);
        }
#endif
        #endregion
    }
}
