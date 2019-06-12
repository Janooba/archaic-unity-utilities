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

        // inspector fields
        [Header("General Values")]
        public AnimationStyle animationStyle;
        public LoopStyle loopStyle;
        public float travelTime = 1f;
        [ShowIf("IsUsingCustomAnim")]
        public AnimationCurve customAnimation;
        public bool isLocalSpace = false;

        [Header("Start Values")]
        public Vector3 startPosition;
        public Quaternion startRotation;

        [Header("End Values")]
        public Vector3 endPosition;
        public Quaternion endRotation;

        // properties
        public bool IsUsingCustomAnim
        {
            get { return AnimationStyle == AnimationStyle.Custom; }
        }

        // private fields
        private float currTime = 0f;
        private float unitTime = 0f;
        private bool isMoving = false;
        private bool isOpen = false;

        // methods
        public IEnumerator Animate(bool isForward)
        {
            isMoving = true;
            currTime = 0;

            if (isForward)
                currTime = 0f;
            else
                currTime = travelTime;

            // While current time is within movement range
            while (currTime <= travelTime && currTime >= 0)
            {
                // Set unit time based on animation style equation
                switch (AnimationStyle)
                {
                    case AnimationStyle.Linear:
                        unitTime = currTime / travelTime;
                        break;

                    case AnimationStyle.Ease:
                        unitTime = Mathf.SmoothStep(0, 1, currTime / travelTime);
                        break;

                    case AnimationStyle.Custom:
                        unitTime = customAnimation.Evaluate(currTime / travelTime);
                        break;
                }

                // Set transform via local space or world space
                SetTransform(Vector3.Lerp(startPosition, endPosition, unitTime), Quaternion.Slerp(startRotation, endRotation, unitTime));

                // Advance time either forwards or backwards
                if (isForward)
                    currTime += Time.deltaTime;
                else
                    currTime -= Time.deltaTime;

                yield return null; // returns next update
            }

            // Animation has reached it's end
            isMoving = false;
            isOpen = !isOpen;

            // Setting to final position incase it hasn't quite reached it yet.
            if (isForward)
                SetTransform(endPosition, endRotation);
            else
                SetTransform(startPosition, startRotation);
        }

        [Button]
        public void Play()
        {
            if (isMoving)
                return;

            StopAllCoroutines();

            switch (LoopStyle)
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
            LoopStyle oldStyle = LoopStyle;
            LoopStyle = LoopStyle.Forward;
            Play();
            LoopStyle = oldStyle;
        }

        public void PlayBackward()
        {
            LoopStyle oldStyle = LoopStyle;
            LoopStyle = LoopStyle.Backward;
            Play();
            LoopStyle = oldStyle;
        }

        [ButtonGroup]
        public void SetStartValues()
        {
            if (isLocalSpace)
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
            if (isLocalSpace)
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

        private void SetTransform(Vector3 position, Quaternion rotation, bool useLocalSpace)
        {
            if (useLocalSpace)
            {
                transform.localPosition = position;
                transform.localRotation = rotation;
            }
            else
            {
                transform.position = position;
                transform.rotation = rotation;
            }
        }
    }
}