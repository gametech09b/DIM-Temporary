using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    public interface IFireable {
        void InitAmmo(AmmoDetailSO ammoDetail, float ammoSpeed, float angle, float weaponAngle, Vector3 weaponDirectionVector, bool isOverrideAmmoMovement = false);

        GameObject GetGameObject();
    }
}
