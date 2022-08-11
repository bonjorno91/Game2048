using Core.Services.AssetProvider;
using Game.Factory.Behaviours;
using Game.StaticData;
using UnityEngine;

namespace Game.Factories
{
    public class UIFactory
    {
        public RectTransform RootTransform => _rootCanvasRectTransform;
        private readonly IAssetProvider _assetProvider;
        private RectTransform _rootCanvasRectTransform;
        private BackgroundFader _backgroundFader;
        private MainMenuView _mainMenu;

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
        }

        private void InitializeResources()
        {
            _rootCanvasRectTransform = Object.Instantiate(_rootCanvasRectTransform);
            _backgroundFader = Object.Instantiate(_backgroundFader, _rootCanvasRectTransform);
            _mainMenu = Object.Instantiate(_mainMenu, _rootCanvasRectTransform);

            _backgroundFader.Initialize();
            _mainMenu.Initialize(_backgroundFader);
            Canvas.ForceUpdateCanvases();

            _mainMenu.gameObject.SetActive(false);
            _backgroundFader.gameObject.SetActive(false);
            _rootCanvasRectTransform.gameObject.SetActive(true);
        }
    }
}