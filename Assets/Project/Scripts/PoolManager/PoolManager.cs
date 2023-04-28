using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DungeonGunner
{
    [DisallowMultipleComponent]
    public class PoolManager : SingletonMonobehaviour<PoolManager>
    {
        [Tooltip("The pools to be created")]
        [SerializeField] private Pool[] poolArray = null;

        private Transform objectPoolTransform;
        private Dictionary<int, Queue<Component>> poolDictionary;



        private void Start()
        {
            objectPoolTransform = this.transform;
            poolDictionary = new Dictionary<int, Queue<Component>>();



            foreach (Pool pool in poolArray)
            {
                CreatePool(pool.prefab, pool.size, pool.componentType);
            }
        }



        private void CreatePool(GameObject prefab, int size, string componentType)
        {
            int key = prefab.GetInstanceID();

            string poolName = $"{prefab.name} Pool";

            GameObject parentPoolGameObject = new GameObject(poolName);
            parentPoolGameObject.transform.SetParent(objectPoolTransform);

            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary.Add(key, new Queue<Component>());

                for (int i = 0; i < size; i++)
                {
                    GameObject newPoolGameObject = Instantiate(prefab, parentPoolGameObject.transform) as GameObject;

                    newPoolGameObject.SetActive(false);

                    Type type = Type.GetType($"{Settings.ProjectName}.{componentType}");

                    poolDictionary[key].Enqueue(newPoolGameObject.GetComponent(type));
                }
            }
        }



        public Component ReuseComponent(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            int key = prefab.GetInstanceID();

            if (poolDictionary.ContainsKey(key))
            {
                Component componentToReuse = GetComponentFromPool(key);

                ResetObject(componentToReuse, prefab, position, rotation);

                return componentToReuse;
            }

            Debug.LogWarning($"Pool with key {key} doesn't exist.");
            return null;
        }



        private Component GetComponentFromPool(int key)
        {
            Component componentToReuse = poolDictionary[key].Dequeue();

            poolDictionary[key].Enqueue(componentToReuse);

            if (componentToReuse.gameObject.activeSelf)
            {
                componentToReuse.gameObject.SetActive(false);
            }

            return componentToReuse;
        }



        private void ResetObject(Component componentToReuse, GameObject prefab, Vector3 position, Quaternion rotation)
        {
            componentToReuse.transform.position = position;
            componentToReuse.transform.rotation = rotation;
            componentToReuse.transform.localScale = prefab.transform.localScale;
        }



        #region Validation
#if UNITY_EDITOR
        private void OnValidate()
        {
            HelperUtilities.CheckEnumerableValue(this, nameof(poolArray), poolArray);
        }
#endif
        #endregion
    }



    [System.Serializable]
    public struct Pool
    {
        public GameObject prefab;
        public int size;
        public string componentType;
    }
}
