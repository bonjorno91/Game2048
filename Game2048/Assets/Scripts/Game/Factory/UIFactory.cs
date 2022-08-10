using Game.Factory.Behaviours;
using Game.LoadState;
using Game.Services.AssetProvider;
using StaticData;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Game.Factory
{
    public class UIFactory
    {
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

        private void InitializeResources()
        {
            _rootCanvasRectTransform = Object.Instantiate(_rootCanvasRectTransform);
            _backgroundFader = Object.Instantiate(_backgroundFader,_rootCanvasRectTransform);
            _mainMenu = Object.Instantiate(_mainMenu,_rootCanvasRectTransform);

            _backgroundFader.Initialize();
            _mainMenu.Initialize(_backgroundFader);
            Canvas.ForceUpdateCanvases();
            
            _mainMenu.gameObject.SetActive(false);
            _backgroundFader.gameObject.SetActive(false);
            _rootCanvasRectTransform.gameObject.SetActive(true);
        }

        public MainMenuView GetMainMenu()
        {
            return _mainMenu;
        }

        private void LoadResources()
        {
            _rootCanvasRectTransform = _assetProvider.LoadAsset<RectTransform>(AssetPath.MenuCanvas);
            _backgroundFader = _assetProvider.LoadAsset<BackgroundFader>(AssetPath.BackgroundFader);
            _mainMenu = _assetProvider.LoadAsset<MainMenuView>(AssetPath.MainMenu);
        }
    }
}