using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archaic.Core.Utilities
{
    /// <summary>
    /// A collection of methods to help determine line of sight and other awareness concepts.
    /// </summary>
    public struct Awareness
    {
        // CanSee functions originally written by Samuel LaRocque. Modified by Kolton Meier

        public static void CanSee(Transform _transform, Transform _target, out bool _canSee, LayerMask _layerMask)
        {
            CanSee(_transform, Vector3.zero, _target, Vector3.zero, out _canSee, "", _layerMask);
        }

        public static void CanSee(Transform _transform, Transform _target, out bool _canSee, string _tag, LayerMask _layerMask)
        {
            CanSee(_transform, Vector3.zero, _target, Vector3.zero, out _canSee, "", _layerMask);
        }

        public static void CanSee(Transform _transform, Vector3 _transformMod, Transform _target, Vector3 _targetMod, out bool _canSee, LayerMask _layerMask)
        {
            CanSee(_transform, _transformMod, _target, _targetMod, out _canSee, "", _layerMask);
        }

        public static void CanSee(Transform _transform, Vector3 _transformMod, Transform _target, Vector3 _targetMod, out bool _canSee, string _tag, LayerMask _layerMask)
        {
            RaycastHit hitInfo;
            _canSee = false;

            //Debug.DrawLine(_transform.position + _transformMod, _target.position + _targetMod, Color.grey, 0.5f);
            if (Physics.Linecast(_transform.position + _transformMod, _target.position + _targetMod, out hitInfo, ~_layerMask))
            {
                // If tag isn't being used, we'll instead look to see if the transform is the same as target.
                // Otherwise if the tag matches...
                _canSee = (_tag == "" && hitInfo.transform == _target) || hitInfo.transform.gameObject.tag == _tag;
            }
        }
    }
}