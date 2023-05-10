using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    public interface IFireable
    {
        void Init(AmmoDetailSO _ammoDetail, float _speed, float _angle, float _weaponAngle, Vector3 _weaponDirectionVector, bool _isOverrideAmmoMovement = false);

        GameObject GetGameObject();
    }
}
