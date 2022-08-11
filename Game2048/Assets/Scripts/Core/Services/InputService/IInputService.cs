using UnityEngine;

namespace Core.Services.InputService
{
    public interface IInputService : IService
    {
        Vector2Int Direction { get; }
        bool IsEnable { get; set; }
    }
}