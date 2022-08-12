using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Factory.Behaviours
{
    public class GameOverView : MonoBehaviour
    {
        [SerializeField] private Button _newGameButton;
        [SerializeField] private Button _exitGameButton;
        private float _scaleDuration;
        public event Action OnNewGameButtonClicked;
        public event Action OnExitGameButtonClicked;

        public void ShowDialog(float scaleDuration)
        {
            _scaleDuration = scaleDuration;
            gameObject.transform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            gameObject.transform.DOScale(Vector3.one, scaleDuration);
        }

        private void Hide()
        {
            gameObject.transform.DOScale(Vector3.zero, _scaleDuration).OnComplete(Deactivate);
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
        }

        #region Mono

        private void OnEnable()
        {
            _newGameButton.onClick.AddListener(OnNewGameButtonClickHandler);
            _exitGameButton.onClick.AddListener(OnExitGameButtonClickHandler);
        }

        private void OnDisable()
        {
            _newGameButton.onClick.RemoveListener(OnNewGameButtonClickHandler);
            _exitGameButton.onClick.RemoveListener(OnExitGameButtonClickHandler);
        }

        private void OnNewGameButtonClickHandler()
        {
            OnNewGameButtonClicked?.Invoke();
            Hide();
        }

        private void OnExitGameButtonClickHandler()
        {
            OnExitGameButtonClicked?.Invoke();
            Hide();
        }

        #endregion
    }
}