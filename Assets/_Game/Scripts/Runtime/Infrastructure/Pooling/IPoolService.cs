using UnityEngine;

namespace GameDemo.Runtime.Infrastructure.Pooling
{
    public interface IPoolService
    {
        GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation);
        void Despawn(GameObject instance);
        void Warmup(GameObject prefab, int count);
    }
}

