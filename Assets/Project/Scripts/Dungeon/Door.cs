using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    #region Requirement Components
    [RequireComponent(typeof(Animator))]
    #endregion
    public class Door : MonoBehaviour {
        [Space(10)]
        [Header("Object References")]


        [Tooltip("The collider of the door.")]
        [SerializeField] private BoxCollider2D doorCollider;


        [HideInInspector] public bool isBossRoomDoor;


        private Animator animator;
        private BoxCollider2D doorTrigger;
        private bool isOpen = false;
        private bool isOpened = false;



        private void Awake() {
            animator = GetComponent<Animator>();
            doorTrigger = GetComponent<BoxCollider2D>();

            doorCollider.enabled = false;
        }



        private void OnEnable() {
            animator.SetBool(Settings.IsOpen, isOpen);
        }



        private void OnTriggerEnter2D(Collider2D other) {
            if (other.CompareTag(Settings.PlayerTag) || other.CompareTag(Settings.PlayerWeaponTag)) {
                OpenDoor();
            }
        }



        private void OpenDoor() {
            if (isOpen) return;

            isOpen = true;
            isOpened = true;
            doorCollider.enabled = false;
            doorTrigger.enabled = false;

            animator.SetBool(Settings.IsOpen, true);
        }



        public void LockDoor() {
            isOpen = false;
            doorCollider.enabled = true;
            doorTrigger.enabled = false;

            animator.SetBool(Settings.IsOpen, false);
        }



        public void UnlockDoor() {
            doorCollider.enabled = false;
            doorTrigger.enabled = true;

            if (isOpened) {
                isOpen = false;
                OpenDoor();
            }
        }



        #region Validation
        private void OnValidate() {
            HelperUtilities.ValidateCheckNullValue(this, nameof(doorCollider), doorCollider);
        }
        #endregion
    }
}
