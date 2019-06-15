using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Archaic.Core.Utilities
{
    /// <summary>
    /// A simple script to handle moving between two positions/rotations. Great for prototyping or quick tweens.
    /// </summary>
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

        [TabGroup("Start Values", GroupID = "Tabs")]
        public Vector3 startPosition;
        [TabGroup("Start Values", GroupID = "Tabs")]
        public Quaternion startRotation;

        [TabGroup("End Values", GroupID = "Tabs")]
        public Vector3 endPosition;
        [TabGroup("End Values", GroupID = "Tabs")]
        public Quaternion endRotation;

        // properties
        public bool IsUsingCustomAnim
        {
            get { return animationStyle == AnimationStyle.Custom; }
        }

        // private fields
        private float currTime = 0f;
        private float unitTime = 0f;
        private bool isMoving = false;
        private bool isOpen = false;
        private Coroutine activeRoutine;

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
                switch (animationStyle)
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
                SetTransform(Vector3.Lerp(startPosition, endPosition, unitTime), Quaternion.Slerp(startRotation, endRotation, unitTime), isLocalSpace);

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
                SetTransform(endPosition, endRotation, isLocalSpace);
            else
                SetTransform(startPosition, startRotation, isLocalSpace);
        }

        [Button(ButtonSizes.Medium)]
        [DisableInEditorMode]
        public void Play()
        {
            if (isMoving)
                return;

            if (activeRoutine != null)
                StopCoroutine(activeRoutine);

            switch (loopStyle)
            {
                case LoopStyle.Forward:
                    activeRoutine = StartCoroutine(Animate(true));
                    break;
                case LoopStyle.PingPong:
                    activeRoutine = StartCoroutine(Animate(!isOpen));
                    break;
                case LoopStyle.Backward:
                    activeRoutine = StartCoroutine(Animate(false));
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

        [TabGroup("Start Values", GroupID = "Tabs")]
        [ButtonGroup("Tabs/Start Values/StartButtons")]
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

        [TabGroup("Start Values", GroupID = "Tabs")]
        [ButtonGroup("Tabs/Start Values/StartButtons")]
        public void MoveToStartValues()
        {
            if (isLocalSpace)
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

        [TabGroup("End Values", GroupID = "Tabs"), Button]
        [ButtonGroup("Tabs/End Values/EndButtons")]
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

        [TabGroup("End Values", GroupID = "Tabs"), Button]
        [ButtonGroup("Tabs/End Values/EndButtons")]
        public void MoveToEndValues()
        {
            if (isLocalSpace)
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