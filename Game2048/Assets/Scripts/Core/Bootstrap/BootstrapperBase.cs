using System;
using UnityEngine;

namespace Core.Bootstrap
{
    public abstract class BootstrapperBase : MonoBehaviour
    {
        private static BootstrapperBase _bootstrapperBase;

        private void Awake()
        {
            if (_bootstrapperBase != null)
            {
                Destroy(gameObject);
                return;
            }

            _bootstrapperBase = this;
            DontDestroyOnLoad(this.gameObject);
            OnLoad();
        }

        protected abstract void OnLoad();
        public virtual event Action<float> OnUpdate;
    }
}