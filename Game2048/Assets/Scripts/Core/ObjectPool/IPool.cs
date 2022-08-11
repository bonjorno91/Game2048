using UnityEngine;

namespace Core.ObjectPool
{
    public interface IPool<T, in TPayload> : IPoolReleasable<T> where T : IPooling<TPayload>
    {
        T Get(Vector3 position,Quaternion quaternion,TPayload payload);
    }
}