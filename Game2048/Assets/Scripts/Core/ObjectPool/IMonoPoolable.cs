using UnityEngine;

namespace Core.ObjectPool
{
    public interface IMonoPoolable<out TPoolable> : IMonoPoolableBase<TPoolable> where TPoolable : Object, IMonoPoolableBase<TPoolable>
    {
        TPoolable OnGetFromPool(Vector3 worldPosition, Quaternion rotation);
    }
    
    public interface IMonoPoolable<out TPoolable, in TPayload> : IMonoPoolableBase<TPoolable> where TPoolable : Object, IMonoPoolableBase<TPoolable>
    {
        TPoolable OnGetFromPool(Vector3 worldPosition, Quaternion rotation, TPayload data);
    }
}