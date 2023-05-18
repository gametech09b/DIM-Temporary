using UnityEngine;
using Cinemachine;

namespace DIM
{
    [RequireComponent(typeof(CinemachineTargetGroup))]
    public class TG_CinemachineTarget : MonoBehaviour
    {
        private CinemachineTargetGroup cinemachineTargetGroup;
        public TG_GameManager gameManager;

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
            Transform currentPlayerTransform = gameManager.GetCurrentPlayer().transform;
            cinemachineTargetGroup.AddMember(currentPlayerTransform, 1, 2.5f);

            cinemachineTargetGroup.AddMember(cursorTargetTransform, 1, 1);
        }
    }
}
