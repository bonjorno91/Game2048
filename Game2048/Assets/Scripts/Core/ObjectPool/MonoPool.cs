using UnityEngine;

namespace Core.ObjectPool
{
    public class MonoPool<T> : MonoPoolBase<T>, IMonoPool<T> where T : Object, IMonoPoolable<T>
    {
        public MonoPool(T instance, int poolSize, Transform parent) : base(instance, poolSize, parent)
        {
            
        }

        public T GetFromPool(Vector3 worldPosition, Quaternion rotation)
        {
            return GetInstanceFromPool().OnGetFromPool(worldPosition,rotation);
        }
    }
}