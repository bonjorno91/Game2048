using Core.FiniteStateMachine;
using Core.Services.AssetProvider;
using Core.Services.InputService;
using Core.Services.SaveLoad;
using Core.Services.UpdateService;
using Game.Configs;
using Game.Factories;
using Game.Factory;
using Game.Services;
using Game.StaticData;
using UnityEngine;

namespace Game.States
{
    public class LoadGameState : IState
    {
        private readonly IInputService _inputService;
        private readonly IUpdateService _updateService;
        private readonly IStateMachine _stateMachine;
        private readonly GameLoopState _gameLoopState;
        private readonly UIFactory _uiFactory;
        private readonly AssetProvider _assetProvider;
        private readonly ISaveLoadService _saveLoadService;
        private readonly GameData _gameData;
        private readonly ISaveLoadServiceProvider<GameData> _gameDataSaveLoadProvider;
        private readonly ApplicationMainMenuState _applicationMainMenuState;
        private readonly GameFactory _gameFactory;

        public LoadGameState(IStateMachine stateMachine,IInputService inputService, IUpdateService updateService)
        {
            _stateMachine = stateMachine;
            _inputService = inputService;
            _updateService = updateService;
            _assetProvider = new AssetProvider();
            _saveLoadService = new SaveLoadService();
            _gameDataSaveLoadProvider = new GameDataSaveLoadServiceProvider(_saveLoadService,GameConstant.DefaultBoardSize);
            _gameData = _gameDataSaveLoadProvider.Load();
            _uiFactory = new UIFactory(_assetProvider);
            _applicationMainMenuState = new ApplicationMainMenuState(_stateMachine, _uiFactory, _gameDataSaveLoadProvider);
            _stateMachine.AddState(_applicationMainMenuState);
            _gameFactory = new GameFactory(_assetProvider, _uiFactory);
            _gameLoopState = new GameLoopState(_stateMachine ,_inputService, _updateService, _gameFactory, _gameDataSaveLoadProvider);
            _stateMachine.AddState(_gameLoopState);
        }
        
        public void OnStateEnter()
        {
            // TODO: show curtain
            _stateMachine.EnterState<ApplicationMainMenuState,GameData>(_gameData);
        }

        public void OnStateExit()
        {
            Debug.Log("Exit load state.");
        }
    }
    
    public class NewGameState :  IState
    {
        public void OnStateExit()
        {
            throw new System.NotImplementedException();
        }

        public void OnStateEnter()
        {
            throw new System.NotImplementedException();
        }
    }
    
    public class ContinuePlayState : IState
    {
        public void OnStateExit()
        {
            throw new System.NotImplementedException();
        }

        public void OnStateEnter()
        {
            throw new System.NotImplementedException();
        }
    }
}