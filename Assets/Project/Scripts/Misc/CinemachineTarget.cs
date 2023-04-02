using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace DungeonGunner {
    [RequireComponent(typeof(CinemachineTargetGroup))]
    public class CinemachineTarget : MonoBehaviour {
        private CinemachineTargetGroup cinemachineTargetGroup;

        [Tooltip("Cursor Target")]
        [SerializeField] private Transform cursorTargetTransform;

        private void Awake() {
            cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
        }



        private void Start() {
            SetCinemachineTargetGroup();
        }



        private void Update() {
            cursorTargetTransform.position = HelperUtilities.GetMouseWorldPosition();
        }



        private void SetCinemachineTargetGroup() {
            Transform currentPlayerTransform = GameManager.Instance.GetCurrentPlayer().transform;
            cinemachineTargetGroup.AddMember(currentPlayerTransform, 1, 2.5f);

            cinemachineTargetGroup.AddMember(cursorTargetTransform, 1, 1);
        }
    }
}
