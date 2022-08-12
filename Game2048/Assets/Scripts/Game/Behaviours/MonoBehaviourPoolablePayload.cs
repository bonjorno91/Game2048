using Core.ObjectPool;
using UnityEngine;

namespace Game.Behaviours
{
    public abstract class MonoBehaviourPoolablePayload<TPoolable, TPayload> : MonoBehaviour,
        IMonoPoolable<TPoolable, TPayload> where TPoolable : Object, IMonoPoolableBase<TPoolable>
    {
        public abstract void OnPoolableInitialize(IMonoPoolInitializable<TPoolable> pool);
        public abstract void OnReturnToPool();
        public abstract TPoolable OnGetFromPool(Vector3 worldPosition, Quaternion rotation, TPayload data);
    }
}