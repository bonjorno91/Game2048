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
        private readonly UIFactory _uiFactory;
        private RectTransform _rootGameBoardCanvasTransform;
        private RectTransform _backgroundTileRectTransform;
        private GameBoardView _gameBoardView;
        private GameBoardTileView _gameBoardTileView;
        private TilePool _tilePool;
        
        public GameFactory(IAssetProvider assetProvider, UIFactory uiFactory)
        {
            _assetProvider = assetProvider;
            _uiFactory = uiFactory;
            LoadResources();
            InitializeResources();
        }

        public GameBoardTileView GetOrCreateGameTileView(Vector3 position,byte power)
        {
            return _tilePool.Get(position, quaternion.identity, power);
        }
        
        public GameBoardView GetOrCreateGameBoardView(GameBoardModel model)
        {
            #region EarlyExit

            if (model == null)
                throw new NullReferenceException();

            if (!_rootGameBoardCanvasTransform)
                throw new NullReferenceException();

            if (!_backgroundTileRectTransform)
                throw new NullReferenceException();

            if (_gameBoardView.gameObject.GetInstanceID() < 0)
                return _gameBoardView;

            #endregion

            _gameBoardView = Object.Instantiate(_gameBoardView, _rootGameBoardCanvasTransform);

            _rootGameBoardCanvasTransform.gameObject.SetActive(true);
            _gameBoardView.gameObject.SetActive(true);

            var backgroundTiles = new RectTransform[model.Tiles.Length];

            for (int y = model.SizeY-1; y >= 0; y--)
            {
                for (int x = 0; x < model.SizeX; x++)
                {
                    var i = y * model.SizeX + x;
                    backgroundTiles[i] =
                        Object.Instantiate(_backgroundTileRectTransform, _gameBoardView.BoardRoot);
                    backgroundTiles[i].name = $"background ({i}) [{x},{y}]";
                }
            }

            Canvas.ForceUpdateCanvases();
            
            _tilePool = new TilePool(_gameBoardTileView, 16, _rootGameBoardCanvasTransform);
            GameBoardTileView[] gameBoardTileViews = new GameBoardTileView[model.Tiles.Length];

            for (uint i = 0; i < model.Tiles.Length; i++)
            {
                if (model.Tiles[i] > 0)
                {
                    gameBoardTileViews[i] = GetOrCreateGameTileView(backgroundTiles[i].position, model.Tiles[i]);
                }
            }
            
            Canvas.ForceUpdateCanvases();

            return _gameBoardView.Initialize(backgroundTiles, gameBoardTileViews);
        }

        private void LoadResources()
        {
            _rootGameBoardCanvasTransform = _assetProvider.LoadAsset<RectTransform>(AssetPath.RootCanvas);
            _gameBoardView = _assetProvider.LoadAsset<GameBoardView>(AssetPath.BoardPrefab);
            _backgroundTileRectTransform = _assetProvider.LoadAsset<RectTransform>(AssetPath.BoardTileBackground);
            _gameBoardTileView = _assetProvider.LoadAsset<GameBoardTileView>(AssetPath.TilePrefab);
        }

        private void InitializeResources()
        {
            _rootGameBoardCanvasTransform = Object.Instantiate(_rootGameBoardCanvasTransform);
            _rootGameBoardCanvasTransform.gameObject.SetActive(false);
        }
    }
}