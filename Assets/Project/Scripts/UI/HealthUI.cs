using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class HealthUI : MonoBehaviour
    {
        private List<GameObject> heartIconList = new List<GameObject>();



        private void OnEnable()
        {
            GameManager.Instance.GetCurrentPlayer().healthEvent.OnHealthChange += HealthEvent_OnHealthChange;
        }



        private void OnDisable()
        {
            GameManager.Instance.GetCurrentPlayer().healthEvent.OnHealthChange -= HealthEvent_OnHealthChange;
        }



        private void HealthEvent_OnHealthChange(HealthEvent _sender, OnHealthChangeEventArgs _args)
        {
            SetHealthUI(_args);
        }



        private void SetHealthUI(OnHealthChangeEventArgs _args)
        {
            ClearHearthIconList();

            int heartIconAmount = Mathf.CeilToInt(_args.healthPercent * 100f / 20f);

            for (int i = 0; i < heartIconAmount; i++)
            {
                GameObject heartIcon = Instantiate(UIResources.Instance.healthIconPrefab, transform);
                heartIconList.Add(heartIcon);

                heartIcon.GetComponent<RectTransform>().anchoredPosition = new Vector2(Settings.UIHeartIconSpacing * i, 0f);
            }
        }



        private void ClearHearthIconList()
        {
            foreach (GameObject heartIcon in heartIconList)
            {
                Destroy(heartIcon);
            }

            heartIconList.Clear();
        }
    }
}
