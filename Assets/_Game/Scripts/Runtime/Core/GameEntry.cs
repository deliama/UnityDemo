using GameDemo.Runtime.Infrastructure.Async;
using GameDemo.Runtime.Infrastructure.Pooling;
using GameDemo.Runtime.Installers;
using GameDemo.Runtime.Configs;
using UnityEngine;

namespace GameDemo.Runtime.Core
{
    /// <summary>
    /// 场景级组合根：Demo 的启动入口。
    /// 挂在首个可玩场景的引导用 GameObject 上，负责创建运行时上下文并驱动 GameLoop。
    /// </summary>
    public sealed class GameEntry : MonoBehaviour
    {
        [SerializeField] private bool dontDestroyOnLoad = true;
        [SerializeField] private CombatDemoConfig combatDemoConfig;

        private GameContext _context;
        private GameLoop _gameLoop;

        private void Awake()
        {
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            // 构建运行时基础服务：对象池、异步加载
            var poolService = new PrefabPoolService();
            var assetLoader = new ResourcesAssetLoader();
            _context = new GameContext(poolService, assetLoader);

            // 通过安装器注册所有系统并初始化
            var installer = new GameInstaller(combatDemoConfig);
            _gameLoop = installer.Build(_context);
            _gameLoop.Initialize();
        }

        private void Update()
        {
            _gameLoop?.Tick(Time.deltaTime);
        }

        private void LateUpdate()
        {
            _gameLoop?.LateTick(Time.deltaTime);
        }

        private void Start()
        {
            Debug.Log("[GameEntry] Demo framework initialized.");
        }

        private void OnDestroy()
        {
            _gameLoop?.Shutdown();
            GameServices.Clear();
        }
    }
}

