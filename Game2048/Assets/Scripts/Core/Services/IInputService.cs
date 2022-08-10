using UnityEngine;

namespace Core.Services
{
    public interface IInputService : IService
    {
        Vector2Int Direction { get; }
        bool IsEnable { get; set; }
    }
}