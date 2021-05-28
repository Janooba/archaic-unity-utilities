using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archaic.Core.Utilities
{
    public class ListBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static List<T> InstanceList = new List<T>();

        protected virtual void OnEnable()
        {
            InstanceList.Add(this as T);
        }

        protected virtual void OnDisable()
        {
            InstanceList.Remove(this as T);
        }
    }
}