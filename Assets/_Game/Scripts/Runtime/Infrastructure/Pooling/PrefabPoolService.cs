using System.Collections.Generic;
using UnityEngine;

namespace GameDemo.Runtime.Infrastructure.Pooling
{
    /// <summary>
    /// 预制体对象池实现：按 prefab 分桶，Spawn 时优先从队列取出，否则 Instantiate；
    /// Despawn 时 SetActive(false) 并回收到队列，避免频繁 Destroy 引发 GC。
    /// </summary>
    public sealed class PrefabPoolService : IPoolService
    {
        /// <summary> 每个预制体对应一个可复用实例队列。 </summary>
        private readonly Dictionary<GameObject, Queue<GameObject>> _poolMap = new Dictionary<GameObject, Queue<GameObject>>();

        /// <summary> 实例 → 预制体映射，Despawn 时据此找到对应队列。 </summary>
        private readonly Dictionary<GameObject, GameObject> _instanceToPrefab = new Dictionary<GameObject, GameObject>();

        /// <summary> 从池中取出或新建实例，设置位置与旋转后返回。 </summary>
        public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
        {
            if (prefab == null)
            {
                return null;
            }

            if (!_poolMap.TryGetValue(prefab, out var queue))
            {
                queue = new Queue<GameObject>();
                _poolMap[prefab] = queue;
            }

            GameObject instance;
            if (queue.Count > 0)
            {
                instance = queue.Dequeue();
                instance.transform.SetPositionAndRotation(position, rotation);
                instance.SetActive(true);
            }
            else
            {
                instance = Object.Instantiate(prefab, position, rotation);
                _instanceToPrefab[instance] = prefab;
            }

            return instance;
        }

        /// <summary> 将实例回收到池中；非本池创建的实例会直接 Destroy。 </summary>
        public void Despawn(GameObject instance)
        {
            if (instance == null)
            {
                return;
            }

            // 非本池创建的实例直接 Destroy
            if (!_instanceToPrefab.TryGetValue(instance, out var prefab))
            {
                Object.Destroy(instance);
                return;
            }

            if (!_poolMap.TryGetValue(prefab, out var queue))
            {
                queue = new Queue<GameObject>();
                _poolMap[prefab] = queue;
            }

            instance.SetActive(false);
            queue.Enqueue(instance);
        }

        /// <summary> 预热：预先实例化 count 个并放入池，减轻首次 Spawn 时的卡顿。 </summary>
        public void Warmup(GameObject prefab, int count)
        {
            if (prefab == null || count <= 0)
            {
                return;
            }

            if (!_poolMap.TryGetValue(prefab, out var queue))
            {
                queue = new Queue<GameObject>();
                _poolMap[prefab] = queue;
            }

            for (var i = 0; i < count; i++)
            {
                var instance = Object.Instantiate(prefab);
                instance.SetActive(false);
                _instanceToPrefab[instance] = prefab;
                queue.Enqueue(instance);
            }
        }
    }
}

