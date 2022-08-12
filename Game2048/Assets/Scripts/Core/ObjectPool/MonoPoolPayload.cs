using UnityEngine;

namespace Core.ObjectPool
{
    public class MonoPoolPayload<TMonoPoolable,TPayload> : MonoPoolBase<TMonoPoolable>, IMonoPool<TMonoPoolable,TPayload> where TMonoPoolable : Object, IMonoPoolable<TMonoPoolable, TPayload>
    {
        public MonoPoolPayload(TMonoPoolable instance, int poolSize, Transform parent) : base(instance, poolSize, parent)
        {
            
        }

        public TMonoPoolable GetFromPool(Vector3 worldPosition, Quaternion rotation, TPayload data)
        {
            return GetInstanceFromPool().OnGetFromPool(worldPosition, rotation, data);
        }
    }
}