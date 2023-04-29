using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class Health : MonoBehaviour
    {
        private int startingAmount;
        private int currentAmount;



        public void SetStartingAmount(int _amount)
        {
            startingAmount = _amount;
            currentAmount = _amount;
        }



        public void SetCurrentAmount(int _amount)
        {
            currentAmount = _amount;
        }



        public int GetStartingAmount()
        {
            return startingAmount;
        }



        public int GetCurrentAmount()
        {
            return currentAmount;
        }
    }
}
