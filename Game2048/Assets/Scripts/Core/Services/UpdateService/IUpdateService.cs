using System;

namespace Core.Services.UpdateService
{
    public interface IUpdateService
    {
        event Action<float> OnUpdate;
    }
}