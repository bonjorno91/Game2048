using Core.FiniteStateMachine;
using Core.Services.AssetProvider;
using Core.Services.InputService;
using Core.Services.SaveLoad;
using Core.Services.UpdateService;
using Game.Behaviours;
using Game.Configs;
using Game.Factories;
using Game.Services;
using Game.StaticData;
using UnityEngine;

namespace Game.States
{
    public class LoadGameState : IState
    {
        private readonly AssetProvider _assetProvider;
        private readonly ExitGameState _exitGameState;
        private readonly GameData _gameData;
        private readonly ISaveLoadServiceProvider<GameData> _gameDataSaveLoadProvider;
        private readonly GameFactory _gameFactory;
        private readonly GameLoopState _gameLoopState;
        private readonly GameOverState _gameOverState;
        private readonly IInputService _inputService;
        private readonly LoadScreenHandler _loadScreenHandler;
        private readonly MainMenuState _mainMenuState;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IStateMachine _stateMachine;
        private readonly UIFactory _uiFactory;
        private readonly IUpdateService _updateService;

        public LoadGameState(IStateMachine stateMachine, IInputService inputService, IUpdateService updateService)
        {
            _stateMachine = stateMachine;
            _inputService = inputService;
            _updateService = updateService;
            _assetProvider = new AssetProvider();
            _saveLoadService = new SaveLoadService();
            _gameDataSaveLoadProvider =
                new GameDataSaveLoadServiceProvider(_saveLoadService);
            _gameData = _gameDataSaveLoadProvider.Load();
            if (_gameData == null) _gameData = GameData.NewGameSession(GameConstant.DefaultBoardSize);
            _uiFactory = new UIFactory(_assetProvider);
            _loadScreenHandler = _uiFactory.GetLoadScreen();
            _mainMenuState =
                new MainMenuState(_stateMachine, _uiFactory, _gameDataSaveLoadProvider);
            _stateMachine.AddState(_mainMenuState);
            _gameFactory = new GameFactory(_assetProvider);
            _gameLoopState = new GameLoopState(_stateMachine, _inputService, _updateService, _gameFactory,
                _gameDataSaveLoadProvider);
            _stateMachine.AddState(_gameLoopState);
            _gameOverState = new GameOverState(_stateMachine, _uiFactory, _gameDataSaveLoadProvider);
            _stateMachine.AddState(_gameOverState);
            _exitGameState = new ExitGameState(_gameDataSaveLoadProvider);
            _stateMachine.AddState(_exitGameState);
        }

        public void OnStateEnter()
        {
            _loadScreenHandler.Show(1);
            _stateMachine.EnterState<MainMenuState, GameData>(_gameData);
        }

        public void OnStateExit()
        {
            Debug.Log("Exit load state.");
        }
    }
}