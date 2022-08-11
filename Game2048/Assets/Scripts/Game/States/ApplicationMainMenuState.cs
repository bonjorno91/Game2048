using Core.DataStructure;
using Core.FiniteStateMachine;
using Game.Configs;
using Game.Factory;
using Game.Factory.Behaviours;
using Game.GameState;
using StaticData;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.ApplicationMainMenu
{
    public class ApplicationMainMenuState : IState<GameData>
    {
        private readonly IStateMachine _stateMachine;
        private readonly UIFactory _uiFactory;
        private readonly ISaveLoadServiceProvider<GameData> _saveLoadService;
        private readonly MainMenuView _mainMenu;
        private GameData _gameData;

        public ApplicationMainMenuState(IStateMachine stateMachine,UIFactory uiFactory,ISaveLoadServiceProvider<GameData> saveLoadService)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
            _saveLoadService = saveLoadService;
            _mainMenu = _uiFactory.GetMainMenu();
        }

        private void OnGameExit()
        {
            _saveLoadService.Save(_gameData);
            #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
            #endif
            Application.Quit(); // TODO: exit code
        }

        private void OnNewGamePlay()
        {
            _gameData.GameBoardModel = GameBoardModel.GetStartBoard(GameConstant.DefaultBoardSize);
            _saveLoadService.Save(_gameData);
            _mainMenu.Hide();
            _stateMachine.EnterState<GameLoopState,GameData>(_gameData);
        }

        private void OnResumePlay()
        {
            _mainMenu.Hide();
            _stateMachine.EnterState<GameLoopState,GameData>(_gameData);
        }

        public void OnStateEnter(GameData payloadData)
        {
            _gameData = payloadData;
            _mainMenu.OnResumeButtonClicked += OnResumePlay;
            _mainMenu.OnNewGameButtonClicked += OnNewGamePlay;
            _mainMenu.OnExitGameButtonClicked += OnGameExit;
            _mainMenu.Show(!payloadData.IsNew);
        }

        public void OnStateExit()
        {
            _mainMenu.OnResumeButtonClicked -= OnResumePlay;
            _mainMenu.OnNewGameButtonClicked -= OnNewGamePlay;
            _mainMenu.OnExitGameButtonClicked -= OnGameExit;
        }
    }
}