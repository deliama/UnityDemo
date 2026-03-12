using GameDemo.Runtime.Infrastructure.Async;
using GameDemo.Runtime.Infrastructure.Pooling;

namespace GameDemo.Runtime.Core
{
    public sealed class GameContext
    {
        public IPoolService PoolService { get; }
        public IAssetLoader AssetLoader { get; }

        public GameContext(IPoolService poolService, IAssetLoader assetLoader)
        {
            PoolService = poolService;
            AssetLoader = assetLoader;
        }
    }
}

