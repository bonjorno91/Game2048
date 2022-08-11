namespace Core.ObjectPool
{
    public interface IPoolingReleasable
    {
        void OnRelease();
    }
}