using UnityEngine;

namespace Archaic.Core.Utilities
{
    /// <summary>
    /// Simple script used for disposable objects that should destroy themselves after a set time.
    /// </summary>
    public class Suicide : MonoBehaviour
    {
        public bool triggerOnStart = true;
        public float delay = 0f;

        private PooledObject pooledObject;

        private PooledObject PooledObject
        {
            get
            {
                if (!pooledObject)
                    pooledObject = GetComponent<PooledObject>();

                return pooledObject;
            }
        }

        // This used to be OnEnable and I don't know why. If you find out, comment here please.
        void Start()
        {
            if (triggerOnStart)
            {
                Trigger();
            }
        }

        public void Trigger()
        {
            if (delay > 0f)
                Invoke("Kill", delay);
            else
                Kill();
        }

        public void Kill()
        {
            if (PooledObject)
                PooledObject.Deactivate();
            else
                Destroy(gameObject);
        }
    }
}