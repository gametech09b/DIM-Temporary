using UnityEngine;

namespace DIM.Environment {
    [DisallowMultipleComponent]
    public class EnvironmentGameObject : MonoBehaviour {
        public SpriteRenderer spriteRenderer;

        // ===================================================================

        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckNullValue(this, nameof(spriteRenderer), spriteRenderer);
        }
#endif
        #endregion
    }
}
