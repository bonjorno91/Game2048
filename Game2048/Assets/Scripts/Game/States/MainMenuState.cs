using Core.FiniteStateMachine;
using Core.Services.SaveLoad;
using Game.Configs;
using Game.Factories;
using Game.Factory.Behaviours;
using Game.StaticData;

namespace Game.States
{
    public class MainMenuState : IState<GameData>
    {
        private readonly MainMenuView _mainMenu;
        private readonly ISaveLoadServiceProvider<GameData> _saveLoadService;
        private readonly IStateMachine _stateMachine;
        private readonly UIFactory _uiFactory;
        private GameData _gameData;

        public MainMenuState(IStateMachine stateMachine, UIFactory uiFactory,
            ISaveLoadServiceProvider<GameData> saveLoadService)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
            _saveLoadService = saveLoadService;
            _mainMenu = _uiFactory.GetMainMenu();
        }

        public void OnStateEnter(GameData payloadData)
        {
            _gameData = payloadData;
            _mainMenu.OnResumeButtonClicked += OnResumePlay;
            _mainMenu.OnNewGameButtonClicked += OnNewGamePlay;
            _mainMenu.OnExitGameButtonClicked += OnGameExit;
            _mainMenu.SetBestScore(_gameData.BestScore.ToString());
            _mainMenu.Show(payloadData != null);
        }

        public void OnStateExit()
        {
            _mainMenu.OnResumeButtonClicked -= OnResumePlay;
            _mainMenu.OnNewGameButtonClicked -= OnNewGamePlay;
            _mainMenu.OnExitGameButtonClicked -= OnGameExit;
        }

        private void OnGameExit()
        {
            _stateMachine.EnterState<ExitGameState, GameData>(_gameData);
        }

        private void OnNewGamePlay()
        {
            _gameData.NewGame(GameConstant.DefaultBoardSize);
            _saveLoadService.Save(_gameData);
            _mainMenu.Hide();
            _stateMachine.EnterState<GameLoopState, GameData>(_gameData);
        }

        private void OnResumePlay()
        {
            _mainMenu.Hide();
            _stateMachine.EnterState<GameLoopState, GameData>(_gameData);
        }
    }
}