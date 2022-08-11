using UnityEngine;

namespace Core.ObjectPool
{
    public interface IPooling<in TPayload> : IPoolingReleasable
    {
        void OnGetFromPool(Vector3 position, Quaternion quaternion, TPayload payload);
    }
}