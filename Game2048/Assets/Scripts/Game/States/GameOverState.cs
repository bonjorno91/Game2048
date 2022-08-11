using Core.DataStructure;
using Core.FiniteStateMachine;
using Game.ApplicationMainMenu;
using Game.Configs;
using StaticData;

namespace Game.GameState
{
    internal class GameOverState : IState<GameData>
    {
        private readonly IStateMachine _stateMachine;
        private readonly ISaveLoadServiceProvider<GameData> _gameDataSaveLoadServiceProvider;
        private GameData _gameData;

        public GameOverState(IStateMachine stateMachine,ISaveLoadServiceProvider<GameData> gameDataSaveLoadServiceProvider)
        {
            _stateMachine = stateMachine;
            _gameDataSaveLoadServiceProvider = gameDataSaveLoadServiceProvider;
        }
        
        public void OnStateEnter(GameData gameData)
        {
            var bestScore = gameData.BestScore = gameData.GameBoardModel.Score > gameData.BestScore
                ? gameData.GameBoardModel.Score
                : gameData.BestScore;
            
            _gameData = new GameData(GameBoardModel.GetStartBoard(GameConstant.DefaultBoardSize),bestScore);
            _gameDataSaveLoadServiceProvider.Save(_gameData);
        }

        public void OnStateExit()
        {
            _stateMachine.EnterState<ApplicationMainMenuState,GameData>(_gameData);
        }
    }
}