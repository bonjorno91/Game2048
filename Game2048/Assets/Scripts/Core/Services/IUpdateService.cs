using System;

namespace Core.Services
{
    public interface IUpdateService
    {
        event Action<float> OnUpdate;
    }
}