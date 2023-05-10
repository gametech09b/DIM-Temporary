using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class LightFlicker : MonoBehaviour
    {
        private Light2D light2D;
        [SerializeField] private float minIntensity;
        [SerializeField] private float maxIntensity;
        [SerializeField] private float minFlickerTime;
        [SerializeField] private float maxFlickerTime;
        private float flickerTimer;



        private void Awake()
        {
            light2D = GetComponent<Light2D>();
        }



        private void Start()
        {
            flickerTimer = Random.Range(minFlickerTime, maxFlickerTime);
        }



        private void Update()
        {
            if (light2D == null)
                return;

            flickerTimer -= Time.deltaTime;

            if (flickerTimer < 0f)
            {
                flickerTimer = Random.Range(minFlickerTime, maxFlickerTime);
                light2D.intensity = Random.Range(minIntensity, maxIntensity);
            }
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckPositiveRange(this, nameof(minIntensity), nameof(maxIntensity), minIntensity, maxIntensity);
            HelperUtilities.CheckPositiveRange(this, nameof(minFlickerTime), nameof(maxFlickerTime), minFlickerTime, maxFlickerTime);
        }
#endif
        #endregion
    }
}
