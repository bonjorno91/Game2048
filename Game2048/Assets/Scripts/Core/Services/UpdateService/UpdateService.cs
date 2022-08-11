using System;
using Core.Bootstrap;

namespace Core.Services.UpdateService
{
    public class UpdateService : IUpdateService
    {
        public event Action<float> OnUpdate;
        private readonly BootstrapperBase _handler;

        public UpdateService(BootstrapperBase handler)
        {
            _handler = handler;
            _handler.OnUpdate += delegate(float deltaTime) { OnUpdate?.Invoke(deltaTime); };
        }
    }
}