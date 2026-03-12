using GameDemo.Runtime.Infrastructure.Async;
using GameDemo.Runtime.Infrastructure.Pooling;

namespace GameDemo.Runtime.Core
{
    /// <summary>
    /// 运行时上下文：持有全局共享的基础服务（对象池、资源加载等），供各系统注入使用。
    /// </summary>
    public sealed class GameContext
    {
        /// <summary> 对象池服务，用于弹幕、特效等频繁生成/回收的实例。 </summary>
        public IPoolService PoolService { get; }

        /// <summary> 异步资源加载服务，可后续替换为 Addressables。 </summary>
        public IAssetLoader AssetLoader { get; }

        public GameContext(IPoolService poolService, IAssetLoader assetLoader)
        {
            PoolService = poolService;
            AssetLoader = assetLoader;
        }
    }
}

