using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    public interface IFireable {
        void InitAmmo(AmmoDetailSO ammoDetail, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 aimDirectionVector, bool isOverrideAmmoMovement = false);

        GameObject GetGameObject();
    }
}
