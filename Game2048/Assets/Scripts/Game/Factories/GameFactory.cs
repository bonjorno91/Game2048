using System;
using Core.DataStructure;
using Core.ObjectPool;
using Core.Services.AssetProvider;
using Game.Behaviours;
using Game.StaticData;
using Unity.Mathematics;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Factories
{
    public class GameFactory
    {
        private readonly IAssetProvider _assetProvider;
        private IMonoPool<BackgroundTile> _backgroundsPool;
        private RectTransform _backgroundTileRectTransform;
        private BackgroundTile[] _backgroundTiles;
        private BackgroundTile _backgroundTileView;
        private GameBoardView _boardView;
        private GameBoardTileView _gameBoardTileView;
        private RectTransform _rootRectTransform;
        private IMonoPool<GameBoardTileView, byte> _tilesPool;

        public GameFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            LoadResources();
            InitializeResources();
        }

        public GameBoardTileView GetOrCreateGameTileView(Vector3 position, byte power)
        {
            return _tilesPool.GetFromPool(position, quaternion.identity, power);
        }

        private void LoadResources()
        {
            _rootRectTransform = _assetProvider.LoadAsset<RectTransform>(AssetPath.RootCanvas);
            _boardView = _assetProvider.LoadAsset<GameBoardView>(AssetPath.BoardPrefab);
            _backgroundTileRectTransform = _assetProvider.LoadAsset<RectTransform>(AssetPath.BoardTileBackground);
            _gameBoardTileView = _assetProvider.LoadAsset<GameBoardTileView>(AssetPath.TilePrefab);
            _backgroundTileView = _assetProvider.LoadAsset<BackgroundTile>(AssetPath.BoardTileBackground);
        }

        public GameBoardView GetGameBoardViewFor(GameBoardModel model)
        {
            #region EarlyExit

            if (model == null)
                throw new NullReferenceException();

            if (!_rootRectTransform)
                throw new NullReferenceException();

            if (!_backgroundTileRectTransform)
                throw new NullReferenceException();

            #endregion

            _rootRectTransform.gameObject.SetActive(true);

            InitializeBackgroundTiles(model);
            InitializeGameBoardTileViews(model);

            return _boardView;
        }

        private void InitializeBackgroundTiles(GameBoardModel model)
        {
            _backgroundTiles = new BackgroundTile[model.Tiles.Length];
            _backgroundsPool.ReturnAll();

            for (var y = 0; y < model.SizeY; y++)
            for (var x = model.SizeX - 1; x >= 0; x--)
            {
                var i = y * model.SizeX + x;
                var backgroundTile = _backgroundsPool.GetFromPool(Vector3.zero, quaternion.identity);

                if (backgroundTile.Index == -1) backgroundTile.Index = i;

                _backgroundTiles[backgroundTile.Index] = backgroundTile;
                _backgroundTiles[backgroundTile.Index].name = $"background ({i}) [{x},{y}]";
            }

            _boardView.Initialize(_backgroundTiles);
            Canvas.ForceUpdateCanvases();
        }

        private void InitializeGameBoardTileViews(GameBoardModel model)
        {
            var gameBoardTileViews = new GameBoardTileView[model.Tiles.Length];
            _tilesPool.ReturnAll();

            for (uint i = 0; i < model.Tiles.Length; i++)
                if (model.Tiles[i] > 0)
                    gameBoardTileViews[i] =
                        GetOrCreateGameTileView(_backgroundTiles[i].RectTransform.position, model.Tiles[i]);

            _boardView.SetGameBoardTileViews(gameBoardTileViews);
        }

        private void InitializeResources()
        {
            _rootRectTransform = Object.Instantiate(_rootRectTransform);
            _boardView = Object.Instantiate(_boardView, _rootRectTransform);
            _backgroundsPool = new MonoPool<BackgroundTile>(_backgroundTileView,
                GameConstant.DefaultGameBoardTileViewsPoolSize, _boardView.BoardRoot);
            _tilesPool = new MonoPoolPayload<GameBoardTileView, byte>(_gameBoardTileView,
                GameConstant.DefaultGameBoardTileViewsPoolSize, _rootRectTransform);
            _rootRectTransform.gameObject.SetActive(false);
        }
    }
}