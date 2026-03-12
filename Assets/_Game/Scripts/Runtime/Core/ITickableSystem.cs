namespace GameDemo.Runtime.Core
{
    /// <summary>
    /// 可驱动系统接口：在 GameLoop 的 Update / LateUpdate 中每帧调用。
    /// 实现此接口的 IGameSystem 会在注册时自动加入 Tick 列表。
    /// </summary>
    public interface ITickableSystem
    {
        /// <summary> 每帧 Update 阶段调用。 </summary>
        void Tick(float deltaTime);

        /// <summary> 每帧 LateUpdate 阶段调用。 </summary>
        void LateTick(float deltaTime);
    }
}

