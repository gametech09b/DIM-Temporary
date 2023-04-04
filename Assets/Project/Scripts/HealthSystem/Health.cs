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



        public void SetStartingAmount(int amount)
        {
            startingAmount = amount;
            currentAmount = amount;
        }



        public void SetCurrentAmount(int amount)
        {
            currentAmount = amount;
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
