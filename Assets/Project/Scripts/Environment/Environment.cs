using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class Environment : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckNullValue(this, nameof(spriteRenderer), spriteRenderer);
        }
#endif
        #endregion
    }
}
