namespace GameDemo.Runtime.Core
{
    /// <summary>
    /// 游戏系统接口：具备初始化与关闭生命周期，可被 GameLoop 统一管理。
    /// </summary>
    public interface IGameSystem
    {
        /// <summary> 系统启动时调用一次。 </summary>
        void Initialize();

        /// <summary> 系统关闭时调用一次，用于释放资源。 </summary>
        void Shutdown();
    }
}

