using UnityEngine;

namespace Core.ObjectPool
{
    public interface IMonoPoolInitializable<in TPoolable> where TPoolable : Object, IMonoPoolableBase<TPoolable>
    {
        void ReturnToPool(TPoolable monoPoolable);
        void ReturnAll();
    }
}