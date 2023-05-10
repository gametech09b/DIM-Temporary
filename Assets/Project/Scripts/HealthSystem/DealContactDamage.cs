using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class DealContactDamage : MonoBehaviour
    {
        [SerializeField] private int damageAmount;

        [SerializeField] private LayerMask layerMask;

        private bool isCollided = false;



        private void OnTriggerEnter2D(Collider2D _other)
        {
            if (isCollided)
                return;

            DealDamage(_other);
        }



        private void OnTriggerStay2D(Collider2D _other)
        {
            if (isCollided)
                return;

            DealDamage(_other);
        }



        private void DealDamage(Collider2D _other)
        {
            int collisionLayerMaskBitshift = 1 << _other.gameObject.layer;

            if ((layerMask.value & collisionLayerMaskBitshift) == 0)
                return;

            TakeContactDamage takeContactDamage = _other.GetComponent<TakeContactDamage>();

            if (takeContactDamage == null)
                return;

            isCollided = true;

            Invoke(nameof(ResetContactCollision), Settings.ContactDamageCooldown);

            takeContactDamage.TakeDamage(damageAmount);
        }



        private void ResetContactCollision()
        {
            isCollided = false;
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckPositiveValue(this, nameof(damageAmount), damageAmount, true);
        }
#endif
        #endregion
    }
}
