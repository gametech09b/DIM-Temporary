using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    public class ControllerHandler : MonoBehaviour {
        [Tooltip("The transform of the player's weapon shoot position")]
        [SerializeField] private Transform weaponShootPointTransform;

        private Player player;



        private void Awake() {
            player = GetComponent<Player>();
        }



        private void Update() {
            MovementInput();

            WeaponInput();
        }



        private void MovementInput() {
            player.idleEvent.CallOnIdleEvent();
        }



        private void WeaponInput() {
            Vector3 weaponDirectionVector;
            float weaponAngle, playerAngle;
            Direction playerDirection;

            HandleWeaponInput(out playerDirection, out playerAngle, out weaponAngle, out weaponDirectionVector);
        }



        private void HandleWeaponInput(out Direction playerDirection, out float playerAngle, out float weaponAngle, out Vector3 weaponDirectionVector) {
            Vector3 mousePosition = HelperUtilities.GetMouseWorldPosition();
            Vector3 playerPosition = transform.position;

            weaponDirectionVector = mousePosition - weaponShootPointTransform.position;

            Vector3 playerDirectionVector = mousePosition - playerPosition;

            weaponAngle = HelperUtilities.GetAngleFromVector(weaponDirectionVector);
            playerAngle = HelperUtilities.GetAngleFromVector(playerDirectionVector);

            playerDirection = HelperUtilities.GetDirectionFromAngle(playerAngle);

            player.aimEvent.CallOnAimEvent(playerDirection, playerAngle, weaponAngle, weaponDirectionVector);
        }
    }
}
