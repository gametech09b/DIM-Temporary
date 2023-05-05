using UnityEngine;

namespace DungeonGunner {
    public class AmmoPattern : MonoBehaviour, IFireable {
        [SerializeField] private AmmoGameObject[] ammoGameObjectArray;

        private SpriteRenderer spriteRenderer;

        private float range;
        private float speed;
        private Vector3 directionVector;
        private float directionAngle;
        private AmmoDetailSO ammoDetail;
        private float chargeTimer;



        private void Update() {
            if (chargeTimer > 0f) {
                chargeTimer -= Time.deltaTime;
                return;
            }

            Vector3 distanceVector = directionVector * speed * Time.deltaTime;

            transform.position += distanceVector;

            transform.Rotate(new Vector3(0f, 0f, ammoDetail.rotationSpeed * Time.deltaTime));

            range -= distanceVector.magnitude;

            if (range < 0f)
                DisableAmmo();
        }



        public GameObject GetGameObject() {
            return gameObject;
        }

        public void Init(AmmoDetailSO _ammoDetail, float _ammoSpeed, float _angle, float _weaponAngle, Vector3 _weaponDirectionVector, bool _isOverrideAmmoMovement = false) {
            this.ammoDetail = _ammoDetail;
            this.speed = _ammoSpeed;

            SetDirection(_ammoDetail, _angle, _weaponAngle, _weaponDirectionVector);

            range = _ammoDetail.range;

            gameObject.SetActive(true);

            foreach (AmmoGameObject ammoGameObject in ammoGameObjectArray) {
                ammoGameObject.Init(_ammoDetail, _ammoSpeed, _angle, _weaponAngle, _weaponDirectionVector, true);
            }

            if (_ammoDetail.chargeTime > 0f)
                chargeTimer = _ammoDetail.chargeTime;
            else
                chargeTimer = 0f;
        }



        private void SetDirection(AmmoDetailSO _ammoDetail, float _aimAngle, float _weaponAimAngle, Vector3 _weaponAimDirectionVector) {
            float randomSpread = Random.Range(_ammoDetail.minSpread, _ammoDetail.maxSpread);

            // get a random spread toggle of 1 or -1
            int randomSpreadToggle = Random.Range(0, 2) == 0 ? 1 : -1;

            if (_weaponAimDirectionVector.magnitude < Settings.AimAngleDistance) {
                directionAngle = _aimAngle;
            } else {
                directionAngle = _weaponAimAngle;
            }

            directionAngle += randomSpread * randomSpreadToggle;

            transform.eulerAngles = new Vector3(0, 0, directionAngle);

            directionVector = HelperUtilities.GetDirectionVectorFromAngle(directionAngle);
        }



        private void DisableAmmo() {
            gameObject.SetActive(false);
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate() {
            HelperUtilities.CheckEnumerableValue(this, nameof(ammoGameObjectArray), ammoGameObjectArray);
        }
#endif
        #endregion
    }
}
