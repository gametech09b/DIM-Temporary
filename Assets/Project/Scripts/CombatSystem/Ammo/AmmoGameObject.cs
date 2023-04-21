using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class AmmoGameObject : MonoBehaviour, IFireable
    {
        [SerializeField] private TrailRenderer trailRenderer;
        private float range;
        private float speed;
        private Vector3 directionVector;
        private float directionAngle;

        private SpriteRenderer spriteRenderer;
        private AmmoDetailSO ammoDetail;

        private float chargeTimer;
        private bool isAmmoMaterialSet;
        private bool isOverrideAmmoMovement;



        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }



        private void Update()
        {
            InitAmmoMaterial();

            if (chargeTimer > 0)
            {
                ProcessChargeTimer();
                return;
            }

            Vector3 distanceVector = directionVector * speed * Time.deltaTime;
            transform.position += distanceVector;
            range -= distanceVector.magnitude;

            if (range < 0)
            {
                DisableAmmo();
            }
        }



        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayHitEffect();
            DisableAmmo();
        }



        private void InitAmmoMaterial()
        {
            if (isAmmoMaterialSet)
            {
                return;
            }

            SetAmmoMaterial(ammoDetail.material);
            isAmmoMaterialSet = true;
        }



        private void ProcessChargeTimer()
        {
            chargeTimer -= Time.deltaTime;
        }



        public GameObject GetGameObject()
        {
            return gameObject;
        }



        public void Init(AmmoDetailSO ammoDetail, float ammoSpeed, float angle, float weaponAngle, Vector3 weaponDirectionVector, bool isOverrideAmmoMovement = false)
        {
            // ammo
            this.ammoDetail = ammoDetail;

            SetDirection(ammoDetail, angle, weaponAngle, weaponDirectionVector);

            spriteRenderer.sprite = ammoDetail.sprite;

            if (ammoDetail.chargeTime > 0)
            {
                chargeTimer = ammoDetail.chargeTime;
                SetAmmoMaterial(ammoDetail.chargeMaterial);
                isAmmoMaterialSet = false;
            }
            else
            {
                chargeTimer = 0;
                SetAmmoMaterial(ammoDetail.material);
                isAmmoMaterialSet = true;
            }

            this.range = ammoDetail.range;

            this.speed = ammoSpeed;

            this.isOverrideAmmoMovement = isOverrideAmmoMovement;

            gameObject.SetActive(true);



            // trail
            if (ammoDetail.isTrailEnabled)
            {
                trailRenderer.gameObject.SetActive(true);
                trailRenderer.emitting = true;
                trailRenderer.material = ammoDetail.trailMaterial;
                trailRenderer.startWidth = ammoDetail.trailStartWidth;
                trailRenderer.endWidth = ammoDetail.trailEndWidth;
                trailRenderer.time = ammoDetail.trailLifetime;
            }
            else
            {
                trailRenderer.emitting = false;
                trailRenderer.gameObject.SetActive(false);
            }
        }



        private void SetDirection(AmmoDetailSO ammoDetail, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
        {
            float randomSpread = Random.Range(ammoDetail.minSpread, ammoDetail.maxSpread);

            // get a random spread toggle of 1 or -1
            int randomSpreadToggle = Random.Range(0, 2) == 0 ? 1 : -1;

            if (weaponAimDirectionVector.magnitude < Settings.AimAngleDistance)
            {
                directionAngle = aimAngle;
            }
            else
            {
                directionAngle = weaponAimAngle;
            }

            directionAngle += randomSpread * randomSpreadToggle;

            transform.eulerAngles = new Vector3(0, 0, directionAngle);

            directionVector = HelperUtilities.GetDirectionVectorFromAngle(directionAngle);
        }



        private void DisableAmmo()
        {
            gameObject.SetActive(false);
        }



        private void SetAmmoMaterial(Material material)
        {
            spriteRenderer.material = material;
        }



        private void PlayHitEffect()
        {
            HitEffectSO hitEffect = ammoDetail.hitEffect;
            if (hitEffect != null && hitEffect.prefab != null)
            {
                HitEffect hitEffectInstance = (HitEffect) PoolManager.Instance.ReuseComponent(hitEffect.prefab, transform.position, Quaternion.identity);

                hitEffectInstance.Init(hitEffect);

                hitEffectInstance.gameObject.SetActive(true);
            }
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.ValidateCheckNullValue(this, nameof(trailRenderer), trailRenderer);
        }
#endif
        #endregion
    }
}
