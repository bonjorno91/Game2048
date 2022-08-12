using UnityEngine;

namespace Core.ObjectPool
{
    public interface IMonoPoolableBase<out TPoolable> where TPoolable : Object, IMonoPoolableBase<TPoolable>
    {
        void OnPoolableInitialize(IMonoPoolInitializable<TPoolable> pool);
        void OnReturnToPool();
    }
}