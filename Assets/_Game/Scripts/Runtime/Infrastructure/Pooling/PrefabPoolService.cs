using System.Collections.Generic;
using UnityEngine;

namespace GameDemo.Runtime.Infrastructure.Pooling
{
    public sealed class PrefabPoolService : IPoolService
    {
        private readonly Dictionary<GameObject, Queue<GameObject>> _poolMap = new Dictionary<GameObject, Queue<GameObject>>();
        private readonly Dictionary<GameObject, GameObject> _instanceToPrefab = new Dictionary<GameObject, GameObject>();

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

        public void Despawn(GameObject instance)
        {
            if (instance == null)
            {
                return;
            }

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

