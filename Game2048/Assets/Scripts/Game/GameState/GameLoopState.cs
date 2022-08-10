using System.Threading.Tasks;
using Core.DataStructure;
using Core.FiniteStateMachine;
using Core.Services;
using Game.Behaviours;
using Game.Controllers;
using Game.Factory;
using Game.Services.AssetProvider;
using UnityEngine;

namespace Game.GameState
{
    public class GameLoopState : IState
    {
        private readonly GameStateConfig _config;
        private readonly IInputService _inputService;
        private readonly IUpdateService _updateService;
        private readonly GameBoardModel _gameBoardModel;
        private readonly GameBoardController _gameBoardController;
        private readonly IAssetProvider _assetProvider;
        private readonly GameFactory _gameFactory;
        private readonly GameBoardView _gameBoardView;
        private IStateMachine _stateMachine;
        private bool IsReady => _gameBoardController.IsReady;
        public bool HasInput => _inputService.Direction.x != 0 || _inputService.Direction.y != 0;
        public bool GameOver => _gameBoardController.GameOver;


        public GameLoopState(GameStateConfig config, IInputService inputService, IUpdateService updateService,
            IAssetProvider assetProvider)
        {
            _config = config;
            _assetProvider = assetProvider;
            _inputService = inputService;
            _updateService = updateService;
            _gameFactory = new GameFactory(_assetProvider);
            _gameBoardModel = GameBoardModel.LoadBoard(config.Field);
            _gameBoardView = _gameFactory.GetOrCreateGameBoardView(_gameBoardModel);
            _gameBoardController = new GameBoardController(_gameBoardModel, _gameBoardView, _gameFactory);
        }

        public void OnStateEnter(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _updateService.OnUpdate += OnLoop;
        }

        private Task _inputTask;

        private void OnLoop(float deltaTime)
        {
            if (GameOver)
            {
                OnGameOver();
            }
            else
            {
                if (HasInput && _inputTask.IsCompleted || _inputTask == null)
                {
                    _inputTask = _gameBoardController.HandleInput(_inputService.Direction);
                }
            }
        }

        private void OnGameOver()
        {
            Debug.Log("Game over!");
        }

        public void OnStateExit()
        {
            // TODO: save data
            Debug.Log("Exit game.");
        }
    }
}