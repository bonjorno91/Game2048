using System.Threading.Tasks;
using Core.DataStructure;
using Game.Behaviours;
using Game.Factories;
using UnityEngine;

namespace Game.Controllers
{
    public class GameBoardController
    {
        private readonly GameBoardModel _gameBoardModel;
        private readonly GameBoardView _gameBoardView;
        private readonly GameFactory _gameFactory;

        public GameBoardController(GameBoardModel gameBoardModel, GameBoardView gameBoardView, GameFactory gameFactory)
        {
            _gameBoardModel = gameBoardModel;
            _gameBoardView = gameBoardView;
            _gameFactory = gameFactory;
            _gameBoardModel.OnTileMoved += OnTileMoveHandler;
            _gameBoardModel.OnTileCollapsed += OnTileCollapsedHandler;
            _gameBoardModel.OnScoreUpdated += OnScoreUpdateHandler;
        }

        public bool GameOver { get; private set; }

        public bool IsReady { get; } = true;

        private void OnScoreUpdateHandler(uint score)
        {
            _gameBoardView.ScoreText = score.ToString();
        }

        private void OnTileCollapsedHandler(int indexFrom, int indexTo, byte power)
        {
            _gameBoardView.CollapseTiles(indexFrom, indexTo, power);
        }

        private void OnTileMoveHandler(int from, int to, byte power)
        {
            _gameBoardView.MoveTile(from, to);
        }

        public async Task HandleInput(Vector2Int direction)
        {
            // Do move
            if (_gameBoardModel.HandleMove(direction))
            {
                // if moved create new tile
                await Task.Delay(50);
                AddRandomTile();
                GameOver = !_gameBoardModel.CanMove();
                await Task.Delay(50);
            }

            await Task.CompletedTask;
        }

        private void AddRandomTile()
        {
            var power = RandomRoll() ? (byte) 2 : (byte) 1;
            var index = _gameBoardModel.AddTileRandom(power);

            if (index >= 0)
            {
                var tile = _gameFactory.GetOrCreateGameTileView(_gameBoardView.GetBackgroundPosition(index), power);
                _gameBoardView.AddNewTile(tile, index);
            }
        }

        private static bool RandomRoll()
        {
            return Random.Range(0, 10) == 5;
        }
    }
}