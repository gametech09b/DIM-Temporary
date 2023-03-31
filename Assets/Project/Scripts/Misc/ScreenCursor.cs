using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner {
    public class ScreenCursor : MonoBehaviour {
        private void Awake() {
            Cursor.visible = false;
        }



        private void Update() {
            transform.position = Input.mousePosition;
        }
    }
}
