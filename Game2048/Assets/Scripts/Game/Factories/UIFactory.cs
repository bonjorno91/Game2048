using Core.Services.AssetProvider;
using Game.Behaviours;
using Game.Factory.Behaviours;
using Game.StaticData;
using UnityEngine;

namespace Game.Factories
{
    public class UIFactory
    {
        private readonly IAssetProvider _assetProvider;
        private BackgroundFader _backgroundFader;
        private GameOverView _gameOverView;
        private LoadScreenHandler _loadScreen;
        private MainMenuView _mainMenu;
        private RectTransform _rootCanvasRectTransform;

        public UIFactory(IAssetProvider assetProvider)
        {
            _assetProvider = assetProvider;
            LoadResources();
            InitializeResources();
        }

        public MainMenuView GetMainMenu()
        {
            _rootCanvasRectTransform.gameObject.SetActive(true);
            return _mainMenu;
        }

        private void LoadResources()
        {
            _rootCanvasRectTransform = _assetProvider.LoadAsset<RectTransform>(AssetPath.MenuCanvas);
            _backgroundFader = _assetProvider.LoadAsset<BackgroundFader>(AssetPath.BackgroundFader);
            _mainMenu = _assetProvider.LoadAsset<MainMenuView>(AssetPath.MainMenu);
            _loadScreen = _assetProvider.LoadAsset<LoadScreenHandler>(AssetPath.LoadScreen);
            _gameOverView = _assetProvider.LoadAsset<GameOverView>(AssetPath.GameOverWindow);
        }

        private void InitializeResources()
        {
            _loadScreen = Object.Instantiate(_loadScreen);
            _rootCanvasRectTransform = Object.Instantiate(_rootCanvasRectTransform);
            _backgroundFader = Object.Instantiate(_backgroundFader, _rootCanvasRectTransform);
            _mainMenu = Object.Instantiate(_mainMenu, _rootCanvasRectTransform);
            _gameOverView = Object.Instantiate(_gameOverView, _rootCanvasRectTransform);
            _backgroundFader.Initialize();
            _mainMenu.Initialize(_backgroundFader);
            Canvas.ForceUpdateCanvases();

            _mainMenu.gameObject.SetActive(false);
            _backgroundFader.gameObject.SetActive(false);
            _gameOverView.gameObject.SetActive(false);
            _rootCanvasRectTransform.gameObject.SetActive(true);
            _loadScreen.gameObject.SetActive(true);
        }

        public LoadScreenHandler GetLoadScreen()
        {
            return _loadScreen;
        }

        public GameOverView GetGameOverWindow()
        {
            return _gameOverView;
        }

        public BackgroundFader GetBackgroundFader()
        {
            return _backgroundFader;
        }
    }
}