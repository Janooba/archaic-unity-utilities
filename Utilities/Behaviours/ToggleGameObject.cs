using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archaic.Core.Utilities
{
    /// <summary>
    /// For use with UnityEvents. Allows the given GameObject to be toggled.
    /// </summary>
    public class ToggleGameObject : MonoBehaviour
    {
        public GameObject objectToToggle;

        /// <summary>
        /// Mostly used with the Unity Event system in-editor. Toggles the active state of this GameObject.
        /// </summary>
        public void ToggleActive()
        {
            if (objectToToggle)
                objectToToggle.SetActive(!objectToToggle.activeSelf);
            else
                gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}