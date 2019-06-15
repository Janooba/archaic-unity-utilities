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
    public class TriggerVolume : MonoBehaviour
    {
        public string[] queryTags;

        public UnityEvent onFirstEnter;
        public UnityEvent onEnter;
        public UnityEvent onExit;
        public UnityEvent onLastExit;

        private List<GameObject> objectsInside = new List<GameObject>();

        private void Start()
        {
            queryTags = queryTags
                .Where(x => x != null)              // Removing any null entries
                .Where(x => x.Trim().Length > 0)    // Trimming empty space before and after tags
                .ToArray();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (DoesTagMatch(other))
            {
                if (objectsInside.Count == 0)
                    onFirstEnter.Invoke();

                onEnter.Invoke();
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

        private bool DoesTagMatch(Collider other)
        {
            bool matched = false;

            if (queryTags.Length == 0)
                matched = true;
            else
                for (int i = 0; i < queryTags.Length; i++)
                {
                    if (other.tag == queryTags[i].Trim())
                        matched = true;
                }
            return matched;
        }
    }
}