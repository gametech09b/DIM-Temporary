using System.Net.Http.Headers;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    public class RandomSpawnableObject<T>
    {
        private struct ChanceRatio
        {
            public T spawnableObject;
            public int minChance;
            public int maxChance;
        }



        private int _ratioTotal = 0;
        private List<ChanceRatio> _chanceRatioList = new List<ChanceRatio>();
        private List<SpawnableObjectsByLevel<T>> _spawnableObjectsByLevelList;



        public RandomSpawnableObject(List<SpawnableObjectsByLevel<T>> spawnableObjectsByLevels)
        {
            this._spawnableObjectsByLevelList = spawnableObjectsByLevels;
        }



        public T GetObject()
        {
            int upperBound = -1;
            _ratioTotal = 0;
            _chanceRatioList.Clear();

            T spawnableObject = default(T);

            foreach (SpawnableObjectsByLevel<T> spawnableObjectsByLevel in _spawnableObjectsByLevelList)
            {
                if (spawnableObjectsByLevel.dungeonLevel == GameManager.Instance.GetCurrentDungeonLevel())
                {
                    foreach (SpawnableObjectRatio<T> spawnableObjectRatio in spawnableObjectsByLevel.spawnableObjectRatioList)
                    {
                        int lowerBound = upperBound + 1;

                        upperBound = lowerBound + spawnableObjectRatio.ratio - 1;

                        _ratioTotal += spawnableObjectRatio.ratio;

                        _chanceRatioList.Add(new ChanceRatio()
                        {
                            spawnableObject = spawnableObjectRatio.spawnableObject,
                            minChance = lowerBound,
                            maxChance = upperBound
                        });
                    }
                }
            }

            if (_chanceRatioList.Count == 0)
                return default(T);

            int lookUpChance = Random.Range(0, _ratioTotal);

            foreach (ChanceRatio spawnChance in _chanceRatioList)
            {
                if (lookUpChance >= spawnChance.minChance && lookUpChance <= spawnChance.maxChance)
                {
                    spawnableObject = spawnChance.spawnableObject;
                    break;
                }
            }

            return spawnableObject;
        }
    }
}
