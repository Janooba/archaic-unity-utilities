﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace Archaic.Core
{
    /// <summary>
    /// An object that can be pooled. Does not work on its own, must be referenced by an Object pool first.
    /// You can add this object to a pool after it has been generated by using the AddOrphan method.
    /// </summary>
    public class PooledObject : MonoBehaviour
    {
        [ReadOnly]
        public ObjectPool parentPool;

        /// <summary> Does this object have a parent pool? </summary>
        public bool IsInitializedInPool { get; private set; }

        /// <summary> Is this object currently in use in the world? </summary>
        public bool IsActiveInPool { get; private set; }

        [FoldoutGroup("Pool Actions"), PropertyOrder(999)] public UnityEvent OnActivate;
        [FoldoutGroup("Pool Actions"), PropertyOrder(999)] public UnityEvent OnDeactivate;

        /// <summary>
        /// Initializes the object for use in it's parent pool
        /// </summary>
        /// <param name="parentPool"></param>
        public virtual void Initialize(ObjectPool parentPool, bool deactivateOnInitialize = true)
        {
            this.parentPool = parentPool;
            IsInitializedInPool = true;

            if (deactivateOnInitialize)
            {
                IsActiveInPool = false;
                transform.SetParent(parentPool.storage, false);
                gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// Activates this object for use
        /// </summary>
        /// <param name="position">Position to "spawn" at</param>
        /// <param name="rotation">Rotation to "spawn" at</param>
        /// <returns></returns>
        public virtual void Activate(Vector3 position, Quaternion rotation, Transform parent)
        {
            transform.SetParent(parent, true);
            transform.position = position;
            transform.rotation = rotation;
            transform.localScale = Vector3.one;
            gameObject.SetActive(true);
            Activate();
        }

        public virtual void Activate()
        {
            if (OnActivate != null)
                OnActivate.Invoke();
            IsActiveInPool = true;

            parentPool.MoveToActive(this);
        }

        /// <summary>
        /// Deactivates this object
        /// </summary>
        public virtual void Deactivate()
        {
            transform.SetParent(parentPool.storage);
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            gameObject.SetActive(false);
            if (OnDeactivate != null)
                OnDeactivate.Invoke();
            IsActiveInPool = false;

            parentPool.MoveToPool(this);
        }
    }
}