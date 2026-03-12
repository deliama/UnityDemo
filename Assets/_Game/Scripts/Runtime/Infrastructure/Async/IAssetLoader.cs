using System.Threading.Tasks;
using UnityEngine;

namespace GameDemo.Runtime.Infrastructure.Async
{
    /// <summary>
    /// 异步资源加载接口：按路径异步加载资源，避免主线程卡顿。
    /// 可实现为 Resources、Addressables 等。
    /// </summary>
    public interface IAssetLoader
    {
        /// <summary> 异步加载指定路径资源，T 通常为 GameObject、Texture、AudioClip 等。 </summary>
        Task<T> LoadAsync<T>(string path) where T : Object;
    }
}

