using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace DungeonGunner
{
    [RequireComponent(typeof(CinemachineTargetGroup))]
    public class CinemachineTarget : MonoBehaviour
    {
        private CinemachineTargetGroup cinemachineTargetGroup;



        private void Awake()
        {
            cinemachineTargetGroup = GetComponent<CinemachineTargetGroup>();
        }



        private void Start()
        {
            SetCinemachineTargetGroup();
        }



        private void SetCinemachineTargetGroup()
        {
            Transform currentPlayerTransform = GameManager.Instance.GetCurrentPlayer().transform;
            cinemachineTargetGroup.AddMember(currentPlayerTransform, 1, 1);
        }
    }
}
