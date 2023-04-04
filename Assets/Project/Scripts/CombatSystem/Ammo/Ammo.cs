using UnityEngine;

namespace DungeonGunner {
    [DisallowMultipleComponent]
    public class Ammo : MonoBehaviour, IFireable {
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



        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }



        private void Update() {
            InitAmmoMaterial();

            ProcessChargeTimer();

            Vector3 distanceVector = directionVector * speed * Time.deltaTime;
            transform.position += distanceVector;
            range -= distanceVector.magnitude;

            if (range < 0) {
                DisableAmmo();
            }
        }



        private void OnTriggerEnter2D(Collider2D other) {
            DisableAmmo();
        }



        private void InitAmmoMaterial() {
            if (isAmmoMaterialSet) {
                return;
            }

            SetAmmoMaterial(ammoDetail.material);
            isAmmoMaterialSet = true;
        }



        private void ProcessChargeTimer() {
            if (chargeTimer <= 0) {
                return;
            }

            chargeTimer -= Time.deltaTime;
        }



        public GameObject GetGameObject() {
            return gameObject;
        }



        public void InitAmmo(AmmoDetailSO ammoDetail, float ammoSpeed, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector, bool isOverrideAmmoMovement = false) {
            // ammo
            this.ammoDetail = ammoDetail;

            SetDirection(ammoDetail, aimAngle, weaponAimAngle, weaponAimDirectionVector);

            spriteRenderer.sprite = ammoDetail.sprite;

            if (ammoDetail.chargeTime > 0) {
                chargeTimer = ammoDetail.chargeTime;
                SetAmmoMaterial(ammoDetail.chargeMaterial);
                isAmmoMaterialSet = false;
            } else {
                chargeTimer = 0;
                SetAmmoMaterial(ammoDetail.material);
                isAmmoMaterialSet = true;
            }

            this.range = ammoDetail.range;

            this.speed = ammoSpeed;

            this.isOverrideAmmoMovement = isOverrideAmmoMovement;

            gameObject.SetActive(true);



            // trail
            if (ammoDetail.isTrailEnabled) {
                trailRenderer.gameObject.SetActive(true);
                trailRenderer.emitting = true;
                trailRenderer.material = ammoDetail.trailMaterial;
                trailRenderer.startWidth = ammoDetail.trailStartWidth;
                trailRenderer.endWidth = ammoDetail.trailEndWidth;
                trailRenderer.time = ammoDetail.trailLifetime;
            } else {
                trailRenderer.emitting = false;
                trailRenderer.gameObject.SetActive(false);
            }
        }



        private void SetDirection(AmmoDetailSO ammoDetail, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector) {
            float randomSpreadAngle = Random.Range(ammoDetail.minSpreadAngle, ammoDetail.maxSpreadAngle);

            // get a random spread toggle of 1 or -1
            int randomSpreadToggle = Random.Range(0, 2) == 0 ? 1 : -1;

            if (weaponAimDirectionVector.magnitude < Settings.AimAngleDistance) {
                directionAngle = aimAngle;
            } else {
                directionAngle = weaponAimAngle;
            }

            directionAngle += randomSpreadAngle * randomSpreadToggle;

            transform.eulerAngles = new Vector3(0, 0, directionAngle);

            directionVector = HelperUtilities.GetDirectionVectorFromAngle(directionAngle);
        }



        private void DisableAmmo() {
            gameObject.SetActive(false);
        }



        private void SetAmmoMaterial(Material material) {
            spriteRenderer.material = material;
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.ValidateCheckNullValue(this, nameof(trailRenderer), trailRenderer);
        }
#endif
        #endregion
    }
}
