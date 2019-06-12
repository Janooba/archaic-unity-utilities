using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Archaic.Core.Utilities
{
    public class SimpleMover : MonoBehaviour
    {
        public enum AnimationStyle { Linear, Ease, Custom };
        public enum LoopStyle { Forward, PingPong, Backward };

        [Header("General Values")]
        public AnimationStyle animationStyle;
        public LoopStyle loopStyle;
        //public bool autoLoop = false;
        public float travelTime = 1f;
        [ShowIf("IsUsingCustomAnim")]
        public AnimationCurve customAnimation;
        public bool localSpace = false;

        [Header("Start Values")]
        public Vector3 startPosition;
        public Quaternion startRotation;

        [Header("End Values")]
        public Vector3 endPosition;
        public Quaternion endRotation;

        private float curTime = 0f;
        private bool isMoving = false;
        private bool isOpen = false;

        private bool IsUsingCustomAnim { get { return animationStyle == AnimationStyle.Custom; } }

        public IEnumerator Animate(bool isForward)
        {
            isMoving = true;
            curTime = 0;

            if (isForward)
                curTime = 0f;
            else
                curTime = travelTime;

            while (curTime <= travelTime && curTime >= 0)
            {
                switch (animationStyle)
                {
                    case AnimationStyle.Linear:
                        if (localSpace)
                        {
                            transform.localPosition = Vector3.Lerp(startPosition, endPosition, curTime / travelTime);
                            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, curTime / travelTime);
                        }
                        else
                        {
                            transform.position = Vector3.Lerp(startPosition, endPosition, curTime / travelTime);
                            transform.rotation = Quaternion.Slerp(startRotation, endRotation, curTime / travelTime);
                        }
                        break;

                    case AnimationStyle.Ease:
                        if (localSpace)
                        {
                            transform.localPosition = Vector3.Lerp(startPosition, endPosition, Mathf.SmoothStep(0, 1, curTime / travelTime));
                            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, Mathf.SmoothStep(0, 1, curTime / travelTime));
                        }
                        else
                        {
                            transform.position = Vector3.Lerp(startPosition, endPosition, Mathf.SmoothStep(0, 1, curTime / travelTime));
                            transform.rotation = Quaternion.Slerp(startRotation, endRotation, Mathf.SmoothStep(0, 1, curTime / travelTime));
                        }
                        break;

                    case AnimationStyle.Custom:
                        if (localSpace)
                        {
                            transform.localPosition = Vector3.Lerp(startPosition, endPosition, customAnimation.Evaluate(curTime / travelTime));
                            transform.localRotation = Quaternion.Slerp(startRotation, endRotation, customAnimation.Evaluate(curTime / travelTime));
                        }
                        else
                        {
                            transform.position = Vector3.Lerp(startPosition, endPosition, customAnimation.Evaluate(curTime / travelTime));
                            transform.rotation = Quaternion.Slerp(startRotation, endRotation, customAnimation.Evaluate(curTime / travelTime));
                        }
                        break;
                }

                if (isForward)
                    curTime += Time.deltaTime;
                else
                    curTime -= Time.deltaTime;

                yield return new WaitForEndOfFrame();
            }

            isMoving = false;
            isOpen = !isOpen;

            // Setting to final position incase it hasn't quite reached it yet.
            if (isForward)
            {
                if (localSpace)
                {
                    transform.localPosition = endPosition;
                    transform.localRotation = endRotation;
                }
                else
                {
                    transform.position = endPosition;
                    transform.rotation = endRotation;
                }
            }
            else
            {
                if (localSpace)
                {
                    transform.localPosition = startPosition;
                    transform.localRotation = startRotation;
                }
                else
                {
                    transform.position = startPosition;
                    transform.rotation = startRotation;
                }
            }
        }

        [Button]
        public void Play()
        {
            if (isMoving)
                return;

            StopAllCoroutines();

            switch (loopStyle)
            {
                case LoopStyle.Forward:
                    StartCoroutine(Animate(true));
                    break;
                case LoopStyle.PingPong:
                    StartCoroutine(Animate(!isOpen));
                    break;
                case LoopStyle.Backward:
                    StartCoroutine(Animate(false));
                    break;
            }
        }

        public void PlayForward()
        {
            LoopStyle oldStyle = loopStyle;
            loopStyle = LoopStyle.Forward;
            Play();
            loopStyle = oldStyle;
        }

        public void PlayBackward()
        {
            LoopStyle oldStyle = loopStyle;
            loopStyle = LoopStyle.Backward;
            Play();
            loopStyle = oldStyle;
        }

        [ButtonGroup]
        public void SetStartValues()
        {
            if (localSpace)
            {
                startPosition = transform.localPosition;
                startRotation = transform.localRotation;
            }
            else
            {
                startPosition = transform.position;
                startRotation = transform.rotation;
            }
        }

        [ButtonGroup]
        public void SetEndValues()
        {
            if (localSpace)
            {
                endPosition = transform.localPosition;
                endRotation = transform.localRotation;
            }
            else
            {
                endPosition = transform.position;
                endRotation = transform.rotation;
            }
        }
    }
}