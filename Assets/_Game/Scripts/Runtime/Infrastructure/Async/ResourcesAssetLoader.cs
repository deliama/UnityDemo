using System.Threading.Tasks;
using UnityEngine;

namespace GameDemo.Runtime.Infrastructure.Async
{
    /// <summary>
    /// Lightweight async loader for early-stage demo.
    /// Can be replaced by Addressables implementation later.
    /// </summary>
    public sealed class ResourcesAssetLoader : IAssetLoader
    {
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

