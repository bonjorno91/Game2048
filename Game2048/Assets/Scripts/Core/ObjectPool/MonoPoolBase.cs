using System.Collections.Generic;
using UnityEngine;

namespace Core.ObjectPool
{
    public abstract class MonoPoolBase<T> : IMonoPoolInitializable<T> where T : Object, IMonoPoolableBase<T>
    {
        private readonly T _instance;
        private readonly Transform _parent;
        private readonly Stack<T> _poolStack;
        private readonly List<T> _poolOutList;

        protected MonoPoolBase(T instance,int poolSize,Transform parent)
        {
            _instance = instance;
            _parent = parent;
            _poolStack = new Stack<T>(poolSize);
            _poolOutList = new List<T>(poolSize);
            PopulatePool(poolSize);
        }

        private void PopulatePool(int poolSize)
        {
            for (int i = 0; i < poolSize; i++)
            {
                var instance = Object.Instantiate(_instance, _parent);
                instance.OnPoolableInitialize(this);
                instance.OnReturnToPool();
                _poolStack.Push(instance);
            }
        }

        public void ReturnToPool(T monoPoolable)
        {
            monoPoolable.OnReturnToPool();
            _poolStack.Push(monoPoolable);
            _poolOutList.Remove(monoPoolable);
        }

        public void ReturnAll()
        {
            foreach (T poolObject in _poolOutList)
            {
                poolObject.OnReturnToPool();
                _poolStack.Push(poolObject);
            }
            
            _poolOutList.Clear();
        }

        protected T GetInstanceFromPool()
        {
            if (_poolStack.Peek() == null)
                IncreasePoolByOne();

            var instance = _poolStack.Pop();
            _poolOutList.Add(instance);
            
            return instance;
        }

        private void IncreasePoolByOne()
        {
            var instance = Object.Instantiate(_instance, _parent);
            instance.OnPoolableInitialize(this);
            _poolOutList.Add(instance);
            ReturnToPool(instance);
        }
    }
}