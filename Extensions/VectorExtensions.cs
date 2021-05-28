using UnityEngine;

namespace Archaic.Core.Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 XYPlane(this Vector3 v) { return new Vector3(v.x, v.y, 0); }
        public static Vector3 XZPlane(this Vector3 v) { return new Vector3(v.x, 0, v.z); }
        public static Vector3 YZPlane(this Vector3 v) { return new Vector3(0, v.y, v.z); }
    }

    /// <summary>A collection of common vector math functions.</summary>
    public struct Mathv
    {
        /// <summary>Interpolates between min and max with smoothing at the limits.</summary>
        public static Vector3 SmoothStep(Vector3 from, Vector3 to, float t)
        {
            return new Vector3(
                Mathf.SmoothStep(from.x, to.x, t),
                Mathf.SmoothStep(from.y, to.y, t),
                Mathf.SmoothStep(from.z, to.z, t));
        }

        /// <summary>
        /// Returns the nearest place on a line to the given object. Line is extended to infinity.
        /// </summary>
        /// <param name="linePnt">Point the line passes through</param>
        /// <param name="lineDir">Unit vector in direction of line, either direction works</param>
        /// <param name="pnt">The point to find nearest on line for</param>
        /// <returns>A single point representing the closest spot to the given object in this line.</returns>
        public static Vector3 NearestPointOnLine(Vector3 linePnt, Vector3 lineDir, Vector3 pnt)
        {
            lineDir.Normalize();//this needs to be a unit vector
            var v = pnt - linePnt;
            var d = Vector3.Dot(v, lineDir);
            return linePnt + lineDir * d;
        }

        /// <summary>
        /// Returns the nearest place on a line segment to the given object.
        /// </summary>
        /// <param name="start">The first point that makes up the line segment</param>
        /// <param name="end">The second point in the line segment</param>
        /// <param name="pnt">The point to find nearest on line for</param>
        /// <returns>A single point representing the closest spot to the given object, clamped to either side.</returns>
        public static Vector3 NearestPointLineSegment(Vector3 start, Vector3 end, Vector3 pnt)
        {
            var line = (end - start);
            var len = line.magnitude;
            line.Normalize();

            var v = pnt - start;
            var d = Vector3.Dot(v, line);
            d = Mathf.Clamp(d, 0f, len);
            return start + line * d;
        }

        public static Vector3 FirstOrderIntercept
        (
            Vector3 shooterPosition,
            Vector3 shooterVelocity,
            float shotSpeed,
            Vector3 targetPosition,
            Vector3 targetVelocity
        )
        {
            Vector3 targetRelativePosition = targetPosition - shooterPosition;
            Vector3 targetRelativeVelocity = targetVelocity - shooterVelocity;
            float t = FirstOrderInterceptTime
            (
                shotSpeed,
                targetRelativePosition,
                targetRelativeVelocity
            );
            return targetPosition + t * (targetRelativeVelocity);
        }

        public static float FirstOrderInterceptTime (float shotSpeed, Vector3 targetRelativePosition, Vector3 targetRelativeVelocity)
        {
            float velocitySquared = targetRelativeVelocity.sqrMagnitude;
            if (velocitySquared < 0.001f)
                return 0f;

            float a = velocitySquared - shotSpeed * shotSpeed;

            //handle similar velocities
            if (Mathf.Abs(a) < 0.001f)
            {
                float t = -targetRelativePosition.sqrMagnitude /
                (
                    2f * Vector3.Dot
                    (
                        targetRelativeVelocity,
                        targetRelativePosition
                    )
                );
                return Mathf.Max(t, 0f); //don't shoot back in time
            }

            float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
            float c = targetRelativePosition.sqrMagnitude;
            float determinant = b * b - 4f * a * c;

            if (determinant > 0f)
            { //determinant > 0; two intercept paths (most common)
                float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                        t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
                if (t1 > 0f)
                {
                    if (t2 > 0f)
                        return Mathf.Min(t1, t2); //both are positive
                    else
                        return t1; //only t1 is positive
                }
                else
                    return Mathf.Max(t2, 0f); //don't shoot back in time
            }
            else if (determinant < 0f) //determinant < 0; no intercept path
                return 0f;
            else //determinant = 0; one intercept path, pretty much never happens
                return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
        }
    }
}