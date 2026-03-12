using System.Threading.Tasks;
using UnityEngine;

namespace GameDemo.Runtime.Infrastructure.Async
{
    /// <summary>
    /// 基于 Resources.LoadAsync 的轻量异步加载器，适用于 Demo 前期。
    /// 后续可替换为 Addressables 实现，以支持分包与远程资源。
    /// </summary>
    public sealed class ResourcesAssetLoader : IAssetLoader
    {
        /// <summary> 异步加载 Resources 下的资源，每帧未完成时 Yield 避免阻塞主线程。 </summary>
        public async Task<T> LoadAsync<T>(string path) where T : Object
        {
            var request = Resources.LoadAsync<T>(path);
            while (!request.isDone)
            {
                await Task.Yield();
            }

            return request.asset as T;
        }
    }
}

