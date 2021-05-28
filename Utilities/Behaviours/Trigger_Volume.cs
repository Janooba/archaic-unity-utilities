using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Archaic.Core.Utilities
{
    /// <summary>
    /// A simple script used to trigger UnityEvents on trigger events.
    /// </summary>
    public class Trigger_Volume : Trigger
    {
        public bool triggerOnce;
        public UnityEvent onFirstEnter;
        public UnityEvent onEnter;
        public UnityEvent onExit;
        public UnityEvent onLastExit;

        private List<GameObject> objectsInside = new List<GameObject>();

        private void OnTriggerEnter(Collider other)
        {
            if (DoesTagMatch(other))
            {
                if (objectsInside.Count == 0)
                    onFirstEnter.Invoke();

                onEnter.Invoke();
                if (triggerOnce)
                    Destroy(gameObject);
                objectsInside.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (objectsInside.Contains(other.gameObject))
            {
                objectsInside.Remove(other.gameObject);
                onExit.Invoke();

                if (objectsInside.Count == 0)
                    onLastExit.Invoke();
            }
        }
    }
}