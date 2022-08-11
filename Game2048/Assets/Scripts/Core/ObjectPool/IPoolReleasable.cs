namespace Core.ObjectPool
{
    public interface IPoolReleasable<T> where T : IPoolingReleasable
    {
        void Release(T releaseObject);
    }
}