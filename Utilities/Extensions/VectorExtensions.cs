using UnityEngine;

namespace Archaic.Core.Utilities
{
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
    }
}