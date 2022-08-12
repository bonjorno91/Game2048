using Core.FiniteStateMachine;
using Core.Services.SaveLoad;
using Game.Behaviours;
using Game.Configs;
using Game.Factories;
using Game.Factory.Behaviours;
using Game.StaticData;

namespace Game.States
{
    internal class GameOverState : IState<GameData>
    {
        private const float ShowupTime = 0.25f;
        private readonly BackgroundFader _backgroundFader;
        private readonly ISaveLoadServiceProvider<GameData> _gameDataSaveLoadServiceProvider;
        private readonly GameOverView _gameOverView;
        private readonly IStateMachine _stateMachine;
        private readonly UIFactory _uiFactory;
        private GameData _gameData;

        public GameOverState(IStateMachine stateMachine, UIFactory uiFactory,
            ISaveLoadServiceProvider<GameData> gameDataSaveLoadServiceProvider)
        {
            _stateMachine = stateMachine;
            _uiFactory = uiFactory;
            _gameDataSaveLoadServiceProvider = gameDataSaveLoadServiceProvider;
            _gameOverView = _uiFactory.GetGameOverWindow();
            _backgroundFader = _uiFactory.GetBackgroundFader();
        }

        public void OnStateEnter(GameData gameData)
        {
            _gameData = gameData;
            _backgroundFader.Show(ShowupTime);
            _gameOverView.ShowDialog(ShowupTime);
            _gameOverView.OnNewGameButtonClicked += OnNewGameButtonClickedHandler;
            _gameOverView.OnExitGameButtonClicked += OnExitGameButtonClickedHandler;
        }

        public void OnStateExit()
        {
            _gameOverView.OnNewGameButtonClicked -= OnNewGameButtonClickedHandler;
            _gameOverView.OnExitGameButtonClicked -= OnExitGameButtonClickedHandler;
            _backgroundFader.Hide(ShowupTime);
        }

        private void OnNewGameButtonClickedHandler()
        {
            _gameData.NewGame(GameConstant.DefaultBoardSize);
            _gameDataSaveLoadServiceProvider.Save(_gameData);
            _stateMachine.EnterState<GameLoopState, GameData>(_gameData);
        }

        private void OnExitGameButtonClickedHandler()
        {
            _gameData.NewGame(GameConstant.DefaultBoardSize);
            _stateMachine.EnterState<ExitGameState, GameData>(_gameData);
        }
    }
}