using UnityEngine;

namespace GameDemo.Runtime.Infrastructure.Pooling
{
    /// <summary>
    /// 对象池服务接口：预制体生成/回收与预热，减少 Instantiate/Destroy 带来的 GC 与卡顿。
    /// </summary>
    public interface IPoolService
    {
        /// <summary> 从池中取出或实例化一个实例，置于指定位置与旋转。 </summary>
        GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation);

        /// <summary> 将实例回收到池中（非池创建则 Destroy）。 </summary>
        void Despawn(GameObject instance);

        /// <summary> 预热：预先实例化 count 个并放入池，减少首次 Spawn 卡顿。 </summary>
        void Warmup(GameObject prefab, int count);
    }
}

