using Core.FiniteStateMachine;
using Core.Services;
using Game.Factory;
using Game.GameState;
using Game.Services.AssetProvider;
using UnityEngine;

namespace Game.LoadState
{
    public class LoadGameState : IState
    {
        private readonly IInputService _inputService;
        private readonly IUpdateService _updateService;
        private IStateMachine _stateMachine;
        private GameLoopState _gameLoopState;
        private UIFactory _uiFactory;
        private AssetProvider _assetProvider;

        public LoadGameState(IInputService inputService, IUpdateService updateService)
        {
            _inputService = inputService;
            _updateService = updateService;
        }
        
        public void OnStateEnter(IStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _assetProvider = new AssetProvider();
            _uiFactory = new UIFactory(_assetProvider);
            var menu = _uiFactory.GetMainMenu();
            menu.Show(Vector3.zero);
            // Try load previous game
            
            // Enter new game
            var config = new GameStateConfig(4);
            _gameLoopState = new GameLoopState(config,_inputService,_updateService, _assetProvider);
            _stateMachine.AddState(_gameLoopState);
            _stateMachine.EnterState<GameLoopState>();
        }

        public void OnStateExit()
        {
            Debug.Log("Exit load state.");
        }
    }
}