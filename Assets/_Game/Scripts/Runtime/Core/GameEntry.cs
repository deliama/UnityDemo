using GameDemo.Runtime.Infrastructure.Async;
using GameDemo.Runtime.Infrastructure.Pooling;
using GameDemo.Runtime.Installers;
using GameDemo.Runtime.Configs;
using UnityEngine;

namespace GameDemo.Runtime.Core
{
    /// <summary>
    /// Scene-level composition root for the demo.
    /// Add this to a bootstrap GameObject in the first playable scene.
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

            var poolService = new PrefabPoolService();
            var assetLoader = new ResourcesAssetLoader();
            _context = new GameContext(poolService, assetLoader);

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

