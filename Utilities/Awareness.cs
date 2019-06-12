using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archaic.Core.Utilities
{
    public struct Awareness
    {
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

            //Debug.DrawLine(_transform.position + _transformMod, _target.position + _targetMod, Color.grey, 0.5f);
            if (Physics.Linecast(_transform.position + _transformMod, _target.position + _targetMod, out hitInfo, ~_layerMask))
            {
                if (_tag == "")
                {
                    if (hitInfo.transform == _target)
                    {
                        _canSee = true;
                    }
                    else
                    {
                        _canSee = false;
                    }
                }
                else
                {
                    if (hitInfo.transform.gameObject.tag == _tag)
                    {
                        _canSee = true;
                    }
                    else
                    {
                        _canSee = false;
                    }
                }
            }
            else
            {
                _canSee = false;
            }
        }
    }
}