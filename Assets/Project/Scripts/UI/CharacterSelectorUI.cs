using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class CharacterSelectorUI : MonoBehaviour {
        [SerializeField] private Transform characterSelectorTransform;

        [SerializeField] private TMP_InputField playerNameInputField;

        private List<PlayerDetailSO> playerDetailList;
        private GameObject playerSelectionPrefab;
        private CurrentPlayerSO currentPlayer;

        private List<GameObject> playerSelectionList = new List<GameObject>();

        private Coroutine updatePlayerSelectionCoroutine;

        private int selectedIndex = 0;
        private float offset = 4f;



        private void Awake() {
            playerDetailList = GameResources.Instance.PlayerDetailList;
            playerSelectionPrefab = GameResources.Instance.PlayerSelectionPrefab;
            currentPlayer = GameResources.Instance.CurrentPlayer;
        }



        private void Start() {
            foreach (PlayerDetailSO playerDetail in playerDetailList) {
                GameObject playerSelectionGameObject = Instantiate(playerSelectionPrefab, characterSelectorTransform);
                playerSelectionList.Add(playerSelectionGameObject);

                playerSelectionGameObject.transform.localPosition = new Vector3(offset * playerDetailList.IndexOf(playerDetail), 0f, 0f);

                PopulatePlayerSelection(playerSelectionGameObject.GetComponent<PlayerSelection>(), playerDetail);
            }

            playerNameInputField.text = currentPlayer.name;

            currentPlayer.playerDetail = playerDetailList[selectedIndex];
        }



        private void PopulatePlayerSelection(PlayerSelection _playerSelection, PlayerDetailSO _playerDetail) {
            _playerSelection.handSpriteRenderer.sprite = _playerDetail.handSprite;
            _playerSelection.handNoWeaponSpriteRenderer.sprite = _playerDetail.handSprite;
            _playerSelection.weaponSpriteRenderer.sprite = _playerDetail.initialWeapon.sprite;
            _playerSelection.animator.runtimeAnimatorController = _playerDetail.runtimeAnimatorController;
        }



        public void NextCharacter() {
            if (selectedIndex >= playerSelectionList.Count - 1)
                return;

            selectedIndex++;

            currentPlayer.playerDetail = playerDetailList[selectedIndex];

            MoveToSelectedCharacter(selectedIndex);
        }



        public void PreviousCharacter() {
            if (selectedIndex <= 0)
                return;

            selectedIndex--;

            currentPlayer.playerDetail = playerDetailList[selectedIndex];

            MoveToSelectedCharacter(selectedIndex);
        }



        private void MoveToSelectedCharacter(int _selectedIndex) {
            if (updatePlayerSelectionCoroutine != null)
                StopCoroutine(updatePlayerSelectionCoroutine);

            updatePlayerSelectionCoroutine = StartCoroutine(UpdatePlayerSelectionCoroutine(_selectedIndex));
        }



        private IEnumerator UpdatePlayerSelectionCoroutine(int _selectedIndex) {
            float xPosition = characterSelectorTransform.localPosition.x;
            float xTargetPosition = _selectedIndex * offset * characterSelectorTransform.localScale.x * -1f;

            while (Mathf.Abs(xPosition - xTargetPosition) > 0.01f) {
                xPosition = Mathf.Lerp(xPosition, xTargetPosition, Time.deltaTime * 10f);

                characterSelectorTransform.localPosition = new Vector3(xPosition, characterSelectorTransform.localPosition.y, 0f);
                yield return null;
            }

            characterSelectorTransform.localPosition = new Vector3(xTargetPosition, characterSelectorTransform.localPosition.y, 0f);
        }

        public void UpdatePlayerName() {
            playerNameInputField.text = playerNameInputField.text.ToUpper();

            currentPlayer.name = playerNameInputField.text;
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckNullValue(this, nameof(characterSelectorTransform), characterSelectorTransform);
            HelperUtilities.CheckNullValue(this, nameof(playerNameInputField), playerNameInputField);
        }
#endif
        #endregion
    }
}
