namespace GameDemo.Runtime.Core
{
    public interface ITickableSystem
    {
        void Tick(float deltaTime);
        void LateTick(float deltaTime);
    }
}

