using UnityEngine;

namespace GameDemo.Runtime.Gameplay.Combat
{
    /// <summary>
    /// 手写数学判定工具：圆形、扇形、AABB，不依赖 Unity 物理 Trigger。
    /// 用于技能范围、普攻判定等，便于面试讲解向量与几何。
    /// </summary>
    public static class HitTestUtility
    {
        /// <summary> 点在水平面圆内（忽略 Y，用 sqrMagnitude 避免开方）。 </summary>
        public static bool IsPointInCircle(Vector3 center, float radius, Vector3 point)
        {
            var delta = point - center;
            delta.y = 0f;
            return delta.sqrMagnitude <= radius * radius;
        }

        /// <summary>
        /// 点在水平面扇形内：先判距离，再判方向（forward 与 toPoint 夹角 ≤ halfAngleDeg）。
        /// 使用点积与 cos 阈值比较，避免 atan2。
        /// </summary>
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

        /// <summary> 点是否在轴对齐包围盒内。 </summary>
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

