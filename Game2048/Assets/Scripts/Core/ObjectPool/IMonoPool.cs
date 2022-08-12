using UnityEngine;

namespace Core.ObjectPool
{
    public interface IMonoPool<TPoolable> : IMonoPoolInitializable<TPoolable> where TPoolable : Object, IMonoPoolable<TPoolable>
    {
        TPoolable GetFromPool(Vector3 worldPosition, Quaternion rotation);
    }
    
        public interface IMonoPool<TPoolable, in TPayload> : IMonoPoolInitializable<TPoolable> where TPoolable : Object, IMonoPoolable<TPoolable,TPayload>
    {
        TPoolable GetFromPool(Vector3 worldPosition, Quaternion rotation,TPayload data);
    }
}