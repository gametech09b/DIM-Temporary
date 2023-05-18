using UnityEngine;

namespace DIM.HealthSystem {
    public class HealthBar : MonoBehaviour {
        [SerializeField] private GameObject healthBar;

        // ===================================================================

        public void EnableHealthBar() {
            gameObject.SetActive(true);
        }



        public void DisableHealthBar() {
            gameObject.SetActive(false);
        }



        public void SetHealthBarValue(float _value) {
            healthBar.transform.localScale = new Vector3(_value, 1f, 1f);
        }
    }
}
