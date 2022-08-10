using UnityEngine;

namespace Core.Services
{
    internal class InputService : IInputService
    {
        public Vector2Int Direction { get; private set; }
        public bool IsEnable { get; set; } = true;
        private readonly IUpdateService _updateService;

        public InputService(IUpdateService updateService)
        {
            _updateService = updateService;
            _updateService.OnUpdate += UpdateService;
        }

        private void UpdateService(float deltaTime)
        {
            Direction= Vector2Int.zero;
            
            if (IsEnable)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    Direction = Vector2Int.up;
                    return;;
                }

                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    Direction = Vector2Int.down;;
                    return;
                }

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    Direction = Vector2Int.left;
                    return;
                }

                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    Direction = Vector2Int.right;
                    return;
                }
            }
        }
    }
}