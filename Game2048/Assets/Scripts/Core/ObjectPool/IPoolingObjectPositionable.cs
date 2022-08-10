using System.Collections.Generic;
using UnityEngine;

namespace Core.ObjectPool
{
    public interface IPoolingTile : IPooling<byte>
    {
        
    }

    public interface IPoolingReleasable
    {
        void OnRelease();
    }

    public interface IPooling<TPayload> : IPoolingReleasable
    {
        void OnGetFromPool(Vector3 position, Quaternion quaternion, TPayload payload);
    }
    
    public interface IPoolReleasable<T> where T : IPoolingReleasable
    {
        void Release(T releaseObject);
    }

    public interface IPool<T,TPayload> : IPoolReleasable<T> where T : IPooling<TPayload>
    {
        T Get(Vector3 position,Quaternion quaternion,TPayload payload);
    }
}