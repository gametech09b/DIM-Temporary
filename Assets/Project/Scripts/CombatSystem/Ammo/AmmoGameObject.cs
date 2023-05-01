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
        private bool isCollided;



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
                DisableAmmo();
        }



        private void OnTriggerEnter2D(Collider2D _other)
        {
            if (isCollided)
                return;

            DealDamage(_other);
            PlayHitEffect();
            DisableAmmo();
        }



        private void DealDamage(Collider2D _other)
        {
            Health collidedHealth = _other.GetComponent<Health>();
            if (collidedHealth != null)
            {
                isCollided = true;
                collidedHealth.TakeDamage(ammoDetail.damage);
            }
        }



        private void InitAmmoMaterial()
        {
            if (isAmmoMaterialSet)
                return;

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



        public void Init(AmmoDetailSO _ammoDetail, float _ammoSpeed, float _angle, float _weaponAngle, Vector3 _weaponDirectionVector, bool _isOverrideAmmoMovement = false)
        {
            #region ammo
            this.ammoDetail = _ammoDetail;

            isCollided = false;

            SetDirection(_ammoDetail, _angle, _weaponAngle, _weaponDirectionVector);

            spriteRenderer.sprite = _ammoDetail.sprite;

            if (_ammoDetail.chargeTime > 0)
            {
                chargeTimer = _ammoDetail.chargeTime;
                SetAmmoMaterial(_ammoDetail.chargeMaterial);
                isAmmoMaterialSet = false;
            }
            else
            {
                chargeTimer = 0;
                SetAmmoMaterial(_ammoDetail.material);
                isAmmoMaterialSet = true;
            }

            this.range = _ammoDetail.range;

            this.speed = _ammoSpeed;

            this.isOverrideAmmoMovement = _isOverrideAmmoMovement;

            gameObject.SetActive(true);
            #endregion


            #region trail
            if (_ammoDetail.isTrailEnabled)
            {
                trailRenderer.gameObject.SetActive(true);
                trailRenderer.emitting = true;
                trailRenderer.material = _ammoDetail.trailMaterial;
                trailRenderer.startWidth = _ammoDetail.trailStartWidth;
                trailRenderer.endWidth = _ammoDetail.trailEndWidth;
                trailRenderer.time = _ammoDetail.trailLifetime;
            }
            else
            {
                trailRenderer.emitting = false;
                trailRenderer.gameObject.SetActive(false);
            }
            #endregion
        }



        private void SetDirection(AmmoDetailSO _ammoDetail, float _aimAngle, float _weaponAimAngle, Vector3 _weaponAimDirectionVector)
        {
            float randomSpread = Random.Range(_ammoDetail.minSpread, _ammoDetail.maxSpread);

            // get a random spread toggle of 1 or -1
            int randomSpreadToggle = Random.Range(0, 2) == 0 ? 1 : -1;

            if (_weaponAimDirectionVector.magnitude < Settings.AimAngleDistance)
            {
                directionAngle = _aimAngle;
            }
            else
            {
                directionAngle = _weaponAimAngle;
            }

            directionAngle += randomSpread * randomSpreadToggle;

            transform.eulerAngles = new Vector3(0, 0, directionAngle);

            directionVector = HelperUtilities.GetDirectionVectorFromAngle(directionAngle);
        }



        private void DisableAmmo()
        {
            gameObject.SetActive(false);
        }



        private void SetAmmoMaterial(Material _material)
        {
            spriteRenderer.material = _material;
        }



        private void PlayHitEffect()
        {
            HitEffectSO hitEffect = ammoDetail.hitEffect;
            if (hitEffect != null && hitEffect.prefab != null)
            {
                HitEffect hitEffectInstance = (HitEffect)PoolManager.Instance.ReuseComponent(hitEffect.prefab, transform.position, Quaternion.identity);

                hitEffectInstance.Init(hitEffect);

                hitEffectInstance.gameObject.SetActive(true);
            }
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckNullValue(this, nameof(trailRenderer), trailRenderer);
        }
#endif
        #endregion
    }
}
