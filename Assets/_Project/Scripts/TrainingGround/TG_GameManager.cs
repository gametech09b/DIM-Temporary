using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DIM.PlayerSystem;

namespace DIM
{
    public class TG_GameManager : SingletonMonobehaviour<GameManager>
    {
        public Player currentPlayer;
        [SerializeField] private PlayerDetailSO currentPlayerDetail;
        private CinemachineTarget cinemachineTarget;

        protected override void Awake()
        {
            base.Awake();
            
            InstantiatePlayer();
            
        }

        private void Start()
        {
            GameResources.Instance.DimmedMaterial.SetFloat("Alpha_Slider", 1f);
        }

        private void InstantiatePlayer() {
            GameObject currentPlayerGameObject = Instantiate(currentPlayerDetail.characterPrefab);

            currentPlayer = currentPlayerGameObject.GetComponent<Player>();
            currentPlayer.Init(currentPlayerDetail);
        }

        private void SetLighting()
        {
            Material material = new Material(GameResources.Instance.VariableLitShader);
            material.SetFloat("Alpha_Slider", 0);
        }

        public Player GetCurrentPlayer()
        {
            return currentPlayer;
        }
    }
}
