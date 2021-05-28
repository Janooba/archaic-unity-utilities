using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Archaic.Core.Extensions
{
    public static class UnityExtensions
    {
        #region Component Extensions
        /// <summary>
        /// Gets the component in either this object, or a parent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public static T GetComponentInThisOrParent<T>(this Component behaviour)
        {
            T component = behaviour.GetComponent<T>();

            if (component == null)
                component = behaviour.GetComponentInParent<T>();

            return component;
        }

        /// <summary>
        /// Gets the components in either this object, or a parent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public static T[] GetComponentsInThisOrParent<T>(this Component behaviour)
        {
            T[] components = behaviour.GetComponents<T>();

            components = components.Union<T>(behaviour.GetComponentsInParent<T>()).ToArray();

            return components;
        }

        /// <summary>
        /// Gets the component in either this object, or a parent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public static T GetComponentInThisOrChildren<T>(this Component behaviour)
        {
            T component = behaviour.GetComponent<T>();

            if (component == null)
                component = behaviour.GetComponentInChildren<T>();

            return component;
        }

        /// <summary>
        /// Gets the components in either this object, or a parent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public static T[] GetComponentsInThisOrChildren<T>(this Component behaviour)
        {
            T[] components = behaviour.GetComponents<T>();
            components = components.Union<T>(behaviour.GetComponentsInChildren<T>()).ToArray();

            return components;
        }
        #endregion

        #region GameObject Extensions

        /// <summary>
        /// Gets the component in either this object, or a parent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public static T GetComponentInThisOrParent<T>(this GameObject behaviour)
        {
            T component = behaviour.GetComponent<T>();

            if (component == null)
                component = behaviour.GetComponentInParent<T>();

            return component;
        }

        /// <summary>
        /// Gets the components in either this object, or a parent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public static T[] GetComponentsInThisOrParent<T>(this GameObject behaviour)
        {
            T[] components = behaviour.GetComponents<T>();
            components = components.Union<T>(behaviour.GetComponentsInParent<T>()).ToArray();

            return components;
        }

        /// <summary>
        /// Gets the component in either this object, or a parent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public static T GetComponentInThisOrChildren<T>(this GameObject behaviour)
        {
            T component = behaviour.GetComponent<T>();

            if (component == null)
                component = behaviour.GetComponentInChildren<T>();

            return component;
        }

        /// <summary>
        /// Gets the components in either this object, or a parent.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="behaviour"></param>
        /// <returns></returns>
        public static T[] GetComponentsInThisOrChildren<T>(this GameObject behaviour)
        {
            T[] components = behaviour.GetComponents<T>();

            if (components == null)
                components = components.Union<T>(behaviour.GetComponentsInChildren<T>()).ToArray();

            return components;
        }

        /// <summary>
        /// Sets the layer of this gameobject, and all of its children
        /// </summary>
        /// <param name="go">root gameObject</param>
        /// <param name="newLayer">new Layer</param>
        public static void SetLayerRecursively(this GameObject go, LayerMask newLayer)
        {
            go.layer = newLayer;

            for (int i = 0; i < go.transform.childCount; i++)
            {
                go.transform.GetChild(i).gameObject.SetLayerRecursively(newLayer);
            }
        }

        /// <summary>
        /// Creates a GameObject as a child, and returns it
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="name">Name to give this new object</param>
        /// <returns>Child GameObject</returns>
        public static GameObject CreateEmptyChild(this GameObject gameObject, string name = "New Object", params Type[] components)
        {
            GameObject newGo = new GameObject(name, components);
            newGo.transform.SetParent(gameObject.transform, false);
            return newGo;
        }

        /// <summary>
        /// Sets the tag of this gameobject, and all of its children
        /// </summary>
        /// <param name="go">root gameObject</param>
        /// <param name="newTag">new Tag</param>
        public static void SetTagRecursively(this GameObject go, string newTag)
        {
            go.tag = newTag;

            foreach (GameObject child in go.transform)
            {
                child.SetTagRecursively(newTag);
            }
        }

        /// <summary>
        /// Copy original component to destination GameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static T CopyComponent<T>(this T original, GameObject destination) where T : Component
        {
            System.Type type = original.GetType();
            Component copy = destination.AddComponent(type);
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy as T;
        }

        /// <summary>
        /// WIP Copy component original values to destination GameObject.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="original"></param>
        /// <param name="destination"></param>
        /// <returns></returns>
        public static T CopyComponentValues<T>(this T original, GameObject destination) where T : Component
        {
            Type type = original.GetType();
            Component copy = destination.GetComponent(type);
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy as T;
        }

        /// <summary>
        /// Shortcut to set the rigidbody to either an active state or an inactive state by modifying the kinematic, collision, and gravity settings.
        /// </summary>
        /// <param name="rbody"></param>
        /// <param name="activeState">True for active in world, False for inactive.</param>
        /// <returns></returns>
        public static Rigidbody SetActive(this Rigidbody rbody, bool activeState)
        {
            if (activeState)
            {
                rbody.isKinematic = false;
                rbody.detectCollisions = true;
                //rbody.useGravity = true;
            }
            else
            {
                rbody.isKinematic = true;
                rbody.detectCollisions = false;
                //rbody.useGravity = false;
            }

            return rbody;
        }
        #endregion

        #region Transform Extensions
        /// <summary>
        /// Resets a transformation to default values
        /// </summary>
        /// <param name="trans"></param>
        public static void ResetTransformation(this Transform trans)
        {
            trans.position = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = new Vector3(1, 1, 1);
        }

        /// <summary>
        /// Creates a GameObject as a child, and returns it
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="name">Name to give this new object</param>
        /// <returns>Child GameObject</returns>
        public static Transform CreateChild(this Transform transform, string name = "New Object", params Type[] components)
        {
            GameObject newGo = new GameObject(name, components);
            newGo.transform.SetParent(transform, false);
            return newGo.transform;
        }

        /// <summary>
        /// Destroys all child gameObjects to this transform
        /// </summary>
        /// <param name="go"></param>
        public static void DestroyChildren(this Transform trans)
        {
            for (int i = trans.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(trans.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Destroys all child gameObjects to this transform
        /// </summary>
        /// <param name="go"></param>
        public static void DestroyChildrenImmediate(this Transform trans)
        {
            for (int i = trans.childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(trans.GetChild(i).gameObject);
            }
        }

        /// <summary>
        /// Punches the transforms rotation randomly
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="shakeAmt">The max degrees to punch (may not always punch this hard)</param>
        /// <param name="shakeZ">Should the Z rotation be punched?</param>
        public static void Shake(this Transform trans, float shakeAmt, bool shakeZ = true)
        {
            Quaternion randomRot = Quaternion.identity;

            randomRot = UnityEngine.Random.rotation;
            if (!shakeZ)
            {
                randomRot = Quaternion.Euler(new Vector3(randomRot.eulerAngles.x, randomRot.eulerAngles.y, 0));
            }

            randomRot = Quaternion.RotateTowards(trans.rotation, randomRot, shakeAmt);
            randomRot = Quaternion.Slerp(trans.rotation, randomRot, shakeAmt * Time.deltaTime);

            trans.rotation = randomRot;
        }

        public static IEnumerator ShakeRoutine(this Transform trans, float shakeAmt, float length, bool taper = false, bool shakeZ = true)
        {
            float time = 0;
            while (time < length)
            {
                time += Time.deltaTime;
                trans.Shake(shakeAmt, shakeZ);
                shakeAmt -= Mathf.Max(length * Time.deltaTime, 0);
                yield return new WaitForEndOfFrame();
            }
        }

        public static void RotateObject(Transform _transform, Transform _target, Vector3 _targetPostionMod, float _speed, float _delta)
        {
            bool _facingTarget;
            RotateObject(_transform, _target, _targetPostionMod, 0, _speed, _delta, out _facingTarget);
        }

        public static void RotateObject(Transform _transform, Transform _target, Vector3 _targetPostionMod, float minRotaion, float _speed, float _delta, out bool _facingTarget)
        {
            Vector3 targetDir = (_target.position + _targetPostionMod) - _transform.position;
            float step = _speed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(_transform.forward, targetDir, step, _delta);
            _transform.rotation = Quaternion.LookRotation(newDir);
            if (Vector3.Angle(targetDir, _transform.forward) < minRotaion)
            {
                _facingTarget = true;
            }
            else
            {
                _facingTarget = false;
            }
        }

        public static void RotateObject(Transform _transform, Vector3 _targetPostion, float minRotaion, float _speed, float _delta, out bool _facingTarget)
        {
            Vector3 targetDir = (_targetPostion) - _transform.position;
            float step = _speed * Time.deltaTime;
            Vector3 newDir = Vector3.RotateTowards(_transform.forward, targetDir, step, _delta);
            _transform.rotation = Quaternion.LookRotation(newDir);
            if (Vector3.Angle(targetDir, _transform.forward) < minRotaion)
            {
                _facingTarget = true;
            }
            else
            {
                _facingTarget = false;
            }
        }
        #endregion

        /// <summary>
        /// Invokes the given method after 'time' in-game time has passed.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="theDelegate"></param>
        /// <param name="time"></param>
        public static void Invoke(this MonoBehaviour me, Action theDelegate, float time, bool useRealTime = false)
        {
            me.StartCoroutine(ExecuteAfterTime(theDelegate, time, useRealTime));
        }

        /// <summary>
        /// Invokes the given method after 'frames' number of frames.
        /// </summary>
        /// <param name="me"></param>
        /// <param name="theDelegate"></param>
        /// <param name="frames"></param>
        public static void Invoke(this MonoBehaviour me, Action theDelegate, int frames)
        {
            me.StartCoroutine(ExecuteAfterFrame(theDelegate, frames));
        }

        private static IEnumerator ExecuteAfterTime(Action theDelegate, float delay, bool useRealTime = false)
        {
            if (useRealTime)
                yield return new WaitForSecondsRealtime(delay);
            else
                yield return new WaitForSeconds(delay);
            theDelegate();
        }

        private static IEnumerator ExecuteAfterFrame(Action theDelegate, int framesToDelay)
        {
            for (int i = 0; i < framesToDelay; i++)
            {
                yield return null;
            }

            theDelegate();
        }
    }
}