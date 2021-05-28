using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Sirenix.OdinInspector;
using Sirenix.Serialization;

namespace Archaic.Core.Utilities
{
    public class Blackboard
    {
        [OdinSerialize, ReadOnly] private Dictionary<string, object> blackboard = new Dictionary<string, object>();

        /// <summary>
        /// Updates info in the blackboard. Adds info if not already found.
        /// </summary>
        /// <param name="info">string: ID, object: Data</param>
        public void UpdateBlackboard(KeyValuePair<string, object> info)
        {
            if (blackboard.ContainsKey(info.Key))
            {
                blackboard[info.Key] = info.Value;
            }
            else
                blackboard.Add(info.Key, info.Value);
        }

        /// <summary>
        /// Updates multiple items in the blackboard. Adds the items if not already found.
        /// </summary>
        /// <param name="suppressWarning"></param>
        /// <param name="info"></param>
        public void UpdateBlackboard(KeyValuePair<string, object>[] info)
        {
            if (info == null)
                return;

            foreach (var pair in info)
            {
                UpdateBlackboard(pair);
            }
        }

        /// <summary>
        /// Check if the value is contained in the blackboard
        /// </summary>
        /// <param name="lookupName"></param>
        /// <returns></returns>
        public bool BlackboardContains(string lookupName)
        {
            return blackboard.ContainsKey(lookupName);
        }

        /// <summary>
        /// Attempts to pull information from the blackboard.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lookupName"></param>
        /// <param name="info">The info requested, if it exists and is of type T</param>
        /// <returns>True if info was successfully found, false if not.</returns>
        public bool TryGetInfo<T>(string lookupName, out T info)
        {
            info = default(T);

            if (!blackboard.ContainsKey(lookupName))
            {
                Debug.LogFormat("Blackboard lookup failed. {0} not found.", lookupName);
                return false;
            }

            if (blackboard[lookupName].GetType() != typeof(T))
            {
                Debug.LogFormat("Blackboard lookup failed. {0} is not of type {1}.", lookupName, info.GetType());
                return false;
            }

            info = (T)blackboard[lookupName];
            return true;
        }
    }
}