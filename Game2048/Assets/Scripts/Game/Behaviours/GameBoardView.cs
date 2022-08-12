using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviours
{
    public class GameBoardView : MonoBehaviour
    {
        [SerializeField] private RectTransform _boardRoot;

        [SerializeField] private TextMeshProUGUI _scoreLabel;

        [SerializeField] private Button _menuButton;

        private BackgroundTile[] _backgroundTiles;

        private GameBoardTileView[] _gameBoardTileViews;

        public string ScoreText
        {
            set => _scoreLabel.text = value;
        }

        public RectTransform BoardRoot => _boardRoot;

        #region MonoCallbacks

        private void OnDestroy()
        {
            if (_backgroundTiles != null)
            {
                foreach (var backgroundTile in _backgroundTiles)
                    if (backgroundTile != null)
                        Destroy(backgroundTile.gameObject);

                _backgroundTiles = null;
            }

            if (_gameBoardTileViews != null)
            {
                foreach (var gameBoardTileView in _gameBoardTileViews)
                    if (gameBoardTileView != null)
                        Destroy(gameBoardTileView.gameObject);

                _gameBoardTileViews = null;
            }
        }

        #endregion

        public event Action OnMenuButtonClicked;

        public void Initialize(BackgroundTile[] backgroundTiles)
        {
            _backgroundTiles = backgroundTiles;
            _menuButton.onClick.AddListener(OnMenuButtonClick);
            _menuButton.gameObject.GetComponent<RectTransform>();
        }

        public void SetGameBoardTileViews(GameBoardTileView[] gameBoardTileViews)
        {
            _gameBoardTileViews = gameBoardTileViews;
        }

        private void OnMenuButtonClick()
        {
            OnMenuButtonClicked?.Invoke();
        }

        public void AddNewTile(GameBoardTileView tileView, int index)
        {
            _gameBoardTileViews[index] = tileView;
            tileView.PlayInitAnimation();
        }

        public Vector3 GetBackgroundPosition(int index)
        {
            return _backgroundTiles[index].RectTransform.position;
        }

        public void MoveTile(int from, int to)
        {
            _gameBoardTileViews[from].MoveTo(_backgroundTiles[to].transform);
            _gameBoardTileViews[to] = _gameBoardTileViews[from];
            _gameBoardTileViews[from] = null;
        }

        public void CollapseTiles(int remove, int indexTo, byte power)
        {
            _gameBoardTileViews[indexTo].Power = power;
            _gameBoardTileViews[indexTo].PlayPowerUpdateAnimation();
            _gameBoardTileViews[remove].DestroyAndReturnPool();
        }

        public void PlayGameOver()
        {
            foreach (var tileView in _gameBoardTileViews)
                if (tileView != null)
                    tileView.PlayGameOver();
        }
    }
}