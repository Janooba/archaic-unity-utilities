using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archaic.Core.Utilities
{
    public interface IUseable
    {
        /// <summary>
        /// Begin using this interactable
        /// </summary>
        /// <returns>False if the item cannot be used right now</returns>
        bool StartUse();

        /// <summary>
        /// Stop using this interactable
        /// </summary>
        void StopUse();
    }
}