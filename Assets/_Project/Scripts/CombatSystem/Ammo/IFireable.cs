using UnityEngine;

namespace DIM.CombatSystem {
    public interface IFireable {
        void Init(AmmoDetailSO _ammoDetail, float _speed, float _angle, float _weaponAngle, Vector3 _weaponDirectionVector, bool _isOverrideAmmoMovement = false);
    }
}
