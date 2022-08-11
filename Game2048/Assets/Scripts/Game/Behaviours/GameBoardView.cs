using System;
using DG.Tweening;
using Game.Factory.Behaviours;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviours
{
    public class GameBoardView : MonoBehaviour
    {
        public string ScoreText
        {
            set => _scoreLabel.text = value;
        }
        public RectTransform BoardRoot => _boardRoot;
        public event Action OnMenuButtonClicked; 
        public RectTransform[] BackgroundTiles => _backgroundTiles;

        [SerializeField] private RectTransform _boardRoot;
        [SerializeField] private TextMeshProUGUI _scoreLabel;
        [SerializeField] private Button _menuButton;
        private RectTransform[] _backgroundTiles;
        private GameBoardTileView[] _gameBoardTileViews;

        public GameBoardView Initialize(RectTransform[] backgroundTiles, GameBoardTileView[] gameBoardTileViews)
        {
            _gameBoardTileViews = gameBoardTileViews;
            _backgroundTiles = backgroundTiles;
            _menuButton.onClick.AddListener(OnMenuButtonClick);
            _menuButton.gameObject.GetComponent<RectTransform>();
            return this;
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
            return _backgroundTiles[index].position;
        }

        public void MoveTile(int from, int to)
        {
            _gameBoardTileViews[from].MoveTo(BackgroundTiles[to].transform);
            _gameBoardTileViews[to] = _gameBoardTileViews[from];
            _gameBoardTileViews[from] = null;
        }

        public void CollapseTiles(int remove, int indexTo, byte power)
        {
            _gameBoardTileViews[indexTo].Power = power;
            _gameBoardTileViews[indexTo].PlayPowerUpdateAnimation();
            _gameBoardTileViews[remove].DestroyAndReturnPool();
        }

        private void OnDestroy()
        {
            if (BackgroundTiles != null)
            {
                foreach (var backgroundTile in BackgroundTiles)
                {
                    if (backgroundTile != null) Destroy(backgroundTile.gameObject);
                }

                _backgroundTiles = null;
            }

            if (_gameBoardTileViews != null)
            {
                foreach (var gameBoardTileView in _gameBoardTileViews)
                {
                    if (gameBoardTileView != null) Destroy(gameBoardTileView.gameObject);
                }

                _gameBoardTileViews = null;
            }
        }
    }
}