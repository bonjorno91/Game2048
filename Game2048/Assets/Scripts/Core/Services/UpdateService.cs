using System;
using Core.Bootstrappers;

namespace Core.Services
{
    public class UpdateService : IUpdateService
    {
        public event Action<float> OnUpdate;
        private readonly Bootstrapper _handler;

        public UpdateService(Bootstrapper handler)
        {
            _handler = handler;
            _handler.OnUpdate += delegate(float deltaTime) { OnUpdate?.Invoke(deltaTime); };
        }
    }
}