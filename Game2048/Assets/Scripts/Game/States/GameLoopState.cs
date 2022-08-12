using System.Threading.Tasks;
using Core.DataStructure;
using Core.FiniteStateMachine;
using Core.Services.InputService;
using Core.Services.SaveLoad;
using Core.Services.UpdateService;
using Game.Behaviours;
using Game.Configs;
using Game.Controllers;
using Game.Factories;
using UnityEngine;

namespace Game.States
{
    public class GameLoopState : IState<GameData>
    {
        private readonly GameFactory _gameFactory;
        private readonly IInputService _inputService;
        private readonly ISaveLoadServiceProvider<GameData> _saveLoadServiceProvider;
        private readonly IStateMachine _stateMachine;
        private readonly IUpdateService _updateService;
        private GameBoardController _gameBoardController;
        private GameBoardView _gameBoardView;
        private GameData _gameData;
        private Task _inputTask;


        public GameLoopState(IStateMachine stateMachine, IInputService inputService, IUpdateService updateService,
            GameFactory gameFactory, ISaveLoadServiceProvider<GameData> saveLoadServiceProvider)
        {
            _stateMachine = stateMachine;
            _inputService = inputService;
            _updateService = updateService;
            _gameFactory = gameFactory;
            _saveLoadServiceProvider = saveLoadServiceProvider;
        }

        private bool HasInput => _inputService.Direction.x != 0 || _inputService.Direction.y != 0;
        private bool GameOver => _gameBoardController.GameOver;

        public void OnStateEnter(GameData payloadData)
        {
            if (!payloadData.IsInitialized)
            {
                payloadData.IsInitialized = true;
                _gameData = payloadData;
                Initialize(_gameData.GameBoardModel);
            }

            _updateService.OnUpdate += OnGameLoop;
            _gameBoardView.OnMenuButtonClicked += OpenMenu;
            Application.quitting += OnQuit;
        }

        public void OnStateExit()
        {
            _updateService.OnUpdate -= OnGameLoop;
            _gameBoardView.OnMenuButtonClicked -= OpenMenu;
            Application.quitting -= OnQuit;
        }

        private void OnQuit()
        {
            _saveLoadServiceProvider.Save(_gameData);
        }

        private void OpenMenu()
        {
            _stateMachine.EnterState<MainMenuState, GameData>(_gameData);
        }

        private void Initialize(GameBoardModel gameBoardModel)
        {
            _gameBoardView = _gameFactory.GetGameBoardViewFor(gameBoardModel);
            _gameBoardView.ScoreText = gameBoardModel.Score.ToString();
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
                if ((HasInput && _inputTask.IsCompleted) || _inputTask == null)
                    _inputTask = _gameBoardController.HandleInput(_inputService.Direction);

                UpdateBestScore();
            }
        }

        private void UpdateBestScore()
        {
            if (_gameData.GameBoardModel.Score > _gameData.BestScore)
                _gameData.BestScore = _gameData.GameBoardModel.Score;
        }

        private void OnGameOver()
        {
            _gameBoardView.PlayGameOver();
            _stateMachine.EnterState<GameOverState, GameData>(_gameData);
        }
    }
}