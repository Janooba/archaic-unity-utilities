using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using System;
using UnityEngine.Events;

namespace Archaic.Core
{
    /// <summary>
    /// This class will generate and manage an object pool for you.
    /// Object pools are generally created using a prefab, but if you 
    /// inherit the pool and pooledObject you can override the methods 
    /// to roll your own system.
    /// 
    /// Don't forget to initialize the pool before you use it!
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        #region Global Pool
        private static Dictionary<string, ObjectPool> globalPools = new Dictionary<string, ObjectPool>();

        /// <summary>
        /// Tries to get the pool with the provided name.
        /// </summary>
        /// <param name="poolName">The name of the pool, case sensitive.</param>
        /// <returns></returns>
        public static ObjectPool GetPool(string poolName)
        {
            ObjectPool pool;
            if (globalPools.TryGetValue(poolName.Trim().ToLower(), out pool))
                return pool;
            else
            {
                Debug.LogErrorFormat("No pool with name {0}", poolName);
                return null;
            }
        }

        /// <summary>
        /// Initializes all pools in the scene that haven't already been initialized and will not self initialize
        /// </summary>
        /// <param name="globalOnly">If true, only global pools will be initialized</param>
        public static void InitializePools(bool globalOnly = true)
        {
            ObjectPool[] pools = FindObjectsOfType<ObjectPool>();

            foreach (var pool in pools)
            {
                // Skip non-global pools
                if (globalOnly && !pool.isGlobal)
                    continue;

                // Skip pools that are already initialized, or will self initialize
                if (pool.IsInitialized || pool.selfInitialize)
                    continue;

                pool.InitializePool();

                if (!pool.IsInitialized)
                    Debug.LogWarning($"Object pool: {pool.name} failed to initialize!");
            }
        }
        #endregion

        [HorizontalGroup("Title"), DisableInPlayMode, Tooltip("Allows this pool to be accessed globally. If set to true, this pool can be accessed through ObjectPool.GetPool(globalAccessor)")]
        public bool isGlobal = false;

        [ValidateInput("ValidateAccessor", DefaultMessage = "Global Accessor cannot be null!")]
        [HorizontalGroup("Title"), HideLabel, SuffixLabel("Global Accessor"), ShowIf("isGlobal"), SerializeField]
        protected string globalAccessor;
        protected bool ValidateAccessor(string val) { return (val != null && val.Trim().Length > 0); }

        [Tooltip("The prefab to use as your pooled objects."), ShowIf("ShowPrefabSlot")]
        public GameObject obj;
        protected virtual bool ShowPrefabSlot { get { return true; } }
        public int poolSize;

        [Tooltip("This is where the unused objects will be stored."), ShowIf("ShowStorageSlot")]
        public Transform storage;
        protected virtual bool ShowStorageSlot { get { return true; } }

        public bool IsInitialized { get { return initialized; } }

        [HorizontalGroup("Init"), DisableInPlayMode]
        public bool selfInitialize = false;
        [HorizontalGroup("Init"), ShowInInspector, ReadOnly]
        protected bool initialized = false;

        [ReadOnly]
        [SerializeField]
        protected List<PooledObject> pool = new List<PooledObject>();

        [ReadOnly]
        [SerializeField]
        protected List<PooledObject> activeObjects = new List<PooledObject>();

        private void OnEnable()
        {
            SetupAccessor();
        }

        private void OnDisable()
        {
            globalPools.Remove(globalAccessor);
        }

        private void Start()
        {
            if (selfInitialize)
                InitializePool();
        }

        /// <summary>
        /// Must be called if this pool isn't set to self initialize. 
        /// This sets the storage for pooled items and adds a base amount to the pool.
        /// </summary>
        public void InitializePool(string accessor = "")
        {
            if (!string.IsNullOrEmpty(accessor))
                globalAccessor = accessor;

            if (isGlobal)
            {
                if (globalAccessor == null)
                {
                    Debug.LogWarning("Global Accessor cannot be null.");
                    return;
                }

                if (globalPools.ContainsKey(globalAccessor))
                {
                    Debug.LogWarning($"Global Accessor {accessor} already exists.");
                    return;
                }
            }

            if (!storage)
                storage = new GameObject("Pool").transform;

            AddToPool(poolSize);

            SetupAccessor();

            initialized = true;
        }

        private void SetupAccessor()
        {
            if (isGlobal)
            {
                var accessor = globalAccessor.Trim().ToLower();

                if (!globalPools.ContainsKey(accessor))
                    globalPools.Add(accessor, this);
            }
        }

        /// <summary>
        /// Deactivates all pooled items, sending them to their storage. Called automatically when the pool is destroyed.
        /// </summary>
        public void DeactivateAll()
        {
            foreach (PooledObject po in pool)
            {
                if (po.IsActiveInPool == true)
                {
                    po.Deactivate();
                }
            }
        }

        /// <summary>
        /// Finds the next disabled object, and resets and activates it. Shortcut if your object doesn't need any extra data
        /// </summary>
        /// <param name="position">Position to spawn object</param>
        /// <param name="rotation">Rotation to spawn object</param>
        /// <returns>The pooled object</returns>
        public virtual PooledObject Activate(Vector3 position, Quaternion rotation, Transform parent)
        {
            if (parent == null)
            {
                Debug.LogError("Pooled object must have parent!");
                return null;
            }

            PooledObject po = GetNext();
            if (po)
            {
                po.Activate(position, rotation, parent);
                return po;
            }

            // If pool is all used, create new entry and return that
            return AddSingleToPool();
        }

        /// <summary>
        /// Returns the next available pooled object. Don't forget to convert it and activate it yourself!
        /// </summary>
        /// <returns></returns>
        public virtual PooledObject GetNext()
        {
            //for (int i = pool.Count - 1; i >= 0; i--)
            //{
            //    if (pool[i] == null)
            //    {
            //        pool.RemoveAt(i);
            //        continue;
            //    }

            //    if (pool[i].IsActiveInPool == false)
            //    {
            //        return pool[i];
            //    }
            //}

            if (pool.Count > 0)
                return pool[0];
            else
                return AddSingleToPool(); // If pool is all used, create new entry and return that
        }

        public void MoveToActive(PooledObject po)
        {
            pool.Remove(po);
            activeObjects.Add(po);
        }

        public void MoveToPool(PooledObject po)
        {
            activeObjects.Remove(po);
            pool.Add(po);
        }

        /// <summary>
        /// Adds amt entries to the projectile pool
        /// </summary>
        /// <param name="amt"></param>
        public void AddToPool(int amt)
        {
            for (int i = 0; i < amt; i++)
            {
                AddSingleToPool();
            }
        }

        /// <summary>
        /// Adds the supplied object to this pool
        /// </summary>
        /// <param name="orphan"></param>
        public void AddOrphan(PooledObject orphan, bool deactivateObject = true)
        {
            pool.Add(orphan);
            orphan.Initialize(this, deactivateObject);
        }

        protected virtual PooledObject AddSingleToPool()
        {
            GameObject go = Instantiate(obj, storage) as GameObject;
            
            PooledObject po = go.GetComponent<PooledObject>();
            if (!po)
            {
                po = go.AddComponent<PooledObject>();
            }

            pool.Add(po);

            po.Initialize(this);
            return po;
        }
    }
}
