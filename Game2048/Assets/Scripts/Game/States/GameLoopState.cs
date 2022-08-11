using System.Threading.Tasks;
using Core.DataStructure;
using Core.FiniteStateMachine;
using Core.Services;
using Game.ApplicationMainMenu;
using Game.Behaviours;
using Game.Configs;
using Game.Controllers;
using Game.Factory;
using UnityEngine;

namespace Game.GameState
{
    public class GameLoopState : IState<GameData>
    {
        private readonly IStateMachine _stateMachine;
        private readonly IInputService _inputService;
        private readonly IUpdateService _updateService;
        private readonly GameFactory _gameFactory;
        private readonly ISaveLoadServiceProvider<GameData> _saveLoadServiceProvider;
        private GameBoardController _gameBoardController;
        private GameBoardView _gameBoardView;
        private bool IsReady => _gameBoardController.IsReady;
        public bool HasInput => _inputService.Direction.x != 0 || _inputService.Direction.y != 0;
        public bool GameOver => _gameBoardController.GameOver;
        private Task _inputTask;
        private GameData _gameData;


        public GameLoopState(IStateMachine stateMachine, IInputService inputService, IUpdateService updateService,
            GameFactory gameFactory,ISaveLoadServiceProvider<GameData> saveLoadServiceProvider)
        {
            _stateMachine = stateMachine;
            _inputService = inputService;
            _updateService = updateService;
            _gameFactory = gameFactory;
            _saveLoadServiceProvider = saveLoadServiceProvider;
        }

        public void OnStateEnter(GameData payloadData)
        {
            if (_gameData == null || !_gameData.GameBoardModel.IsEqual(payloadData.GameBoardModel))
            {
                _gameData = payloadData;
                Initialize(_gameData.GameBoardModel);
            }

            _updateService.OnUpdate += OnGameLoop;
            _gameBoardView.OnMenuButtonClicked += OpenMenu;
            Application.quitting += OnQuit;
        }

        private void OnQuit()
        {
            _saveLoadServiceProvider.Save(_gameData);
        }

        private void OpenMenu()
        {
            _stateMachine.EnterState<ApplicationMainMenuState,GameData>(_gameData);
        }

        public void OnStateExit()
        {
            _updateService.OnUpdate -= OnGameLoop;
            _gameBoardView.OnMenuButtonClicked -= OpenMenu;
            Application.quitting -= OnQuit;
        }

        private void Initialize(GameBoardModel gameBoardModel)
        {
            _gameBoardView = _gameFactory.GetOrCreateGameBoardView(gameBoardModel);
            _gameBoardController = new GameBoardController(gameBoardModel, _gameBoardView, _gameFactory);
        }

        private void OnGameLoop(float deltaTime)
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
            _stateMachine.EnterState<GameOverState, GameData>(_gameData);
        }
    }
}