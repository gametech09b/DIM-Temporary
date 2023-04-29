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



        private void CreatePool(GameObject _prefab, int _size, string _componentType)
        {
            int key = _prefab.GetInstanceID();

            string poolName = $"{_prefab.name} Pool";

            GameObject parentPoolGameObject = new GameObject(poolName);
            parentPoolGameObject.transform.SetParent(objectPoolTransform);

            if (!poolDictionary.ContainsKey(key))
            {
                poolDictionary.Add(key, new Queue<Component>());

                for (int i = 0; i < _size; i++)
                {
                    GameObject newPoolGameObject = Instantiate(_prefab, parentPoolGameObject.transform) as GameObject;

                    newPoolGameObject.SetActive(false);

                    Type type = Type.GetType($"{Settings.ProjectName}.{_componentType}");

                    poolDictionary[key].Enqueue(newPoolGameObject.GetComponent(type));
                }
            }
        }



        public Component ReuseComponent(GameObject _prefab, Vector3 _position, Quaternion _rotation)
        {
            int key = _prefab.GetInstanceID();

            if (poolDictionary.ContainsKey(key))
            {
                Component componentToReuse = GetComponentFromPool(key);

                ResetObject(componentToReuse, _prefab, _position, _rotation);

                return componentToReuse;
            }

            Debug.LogWarning($"Pool with key {key} doesn't exist.");
            return null;
        }



        private Component GetComponentFromPool(int _key)
        {
            Component componentToReuse = poolDictionary[_key].Dequeue();

            poolDictionary[_key].Enqueue(componentToReuse);

            if (componentToReuse.gameObject.activeSelf)
            {
                componentToReuse.gameObject.SetActive(false);
            }

            return componentToReuse;
        }



        private void ResetObject(Component _componentToReuse, GameObject _prefab, Vector3 _position, Quaternion _rotation)
        {
            _componentToReuse.transform.position = _position;
            _componentToReuse.transform.rotation = _rotation;
            _componentToReuse.transform.localScale = _prefab.transform.localScale;
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
