using UnityEngine;

namespace GameDemo.Runtime.Gameplay.Combat
{
    public static class HitTestUtility
    {
        public static bool IsPointInCircle(Vector3 center, float radius, Vector3 point)
        {
            var delta = point - center;
            delta.y = 0f;
            return delta.sqrMagnitude <= radius * radius;
        }

        public static bool IsPointInSector(
            Vector3 origin,
            Vector3 forward,
            float radius,
            float halfAngleDeg,
            Vector3 point)
        {
            var toPoint = point - origin;
            toPoint.y = 0f;

            if (toPoint.sqrMagnitude > radius * radius)
            {
                return false;
            }

            var flatForward = forward;
            flatForward.y = 0f;
            if (flatForward.sqrMagnitude < 0.0001f)
            {
                return false;
            }

            var cosThreshold = Mathf.Cos(halfAngleDeg * Mathf.Deg2Rad);
            var dot = Vector3.Dot(flatForward.normalized, toPoint.normalized);
            return dot >= cosThreshold;
        }

        public static bool IsPointInAabb(Vector3 min, Vector3 max, Vector3 point)
        {
            return point.x >= min.x &&
                   point.x <= max.x &&
                   point.y >= min.y &&
                   point.y <= max.y &&
                   point.z >= min.z &&
                   point.z <= max.z;
        }
    }
}

