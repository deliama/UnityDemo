using System.Threading.Tasks;
using UnityEngine;

namespace GameDemo.Runtime.Infrastructure.Async
{
    public interface IAssetLoader
    {
        Task<T> LoadAsync<T>(string path) where T : Object;
    }
}

