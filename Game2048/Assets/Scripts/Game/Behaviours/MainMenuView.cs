using System;
using DG.Tweening;
using Game.Behaviours;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Factory.Behaviours
{
    [RequireComponent(typeof(RectTransform))]
    public class MainMenuView : MonoBehaviour
    {
        private const float FaderDuration = 0.3f;
        private const float ScaleDuration = 0.3f;
        private const float MoveDuration = 0.3f;
        [SerializeField] private RectTransform _windowRectTransform;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button exitGameButton;
        [SerializeField] private TextMeshProUGUI bestScore;
        private BackgroundFader _backgroundFader;
        public event Action OnResumeButtonClicked;
        public event Action OnNewGameButtonClicked;
        public event Action OnExitGameButtonClicked;

        public void Initialize(BackgroundFader backgroundFader)
        {
            _backgroundFader = backgroundFader;
            if (resumeButton) resumeButton.onClick.AddListener(OnResume);
            if (newGameButton) newGameButton.onClick.AddListener(OnNewGame);
            if (exitGameButton) exitGameButton.onClick.AddListener(OnGameExit);
        }

        public void SetBestScore(string score)
        {
            if (bestScore) bestScore.text = score;
        }


        private void OnGameExit()
        {
            OnExitGameButtonClicked?.Invoke();
        }

        private void OnNewGame()
        {
            OnNewGameButtonClicked?.Invoke();
        }

        private void OnResume()
        {
            OnResumeButtonClicked?.Invoke();
            Hide();
        }


        public void Show(bool continuePlay)
        {
            resumeButton.gameObject.SetActive(continuePlay);
            _windowRectTransform.localScale = Vector3.one;
            _windowRectTransform.localPosition = Vector3.up * 1000;

            gameObject.SetActive(true);
            _backgroundFader.Show(FaderDuration);

            _windowRectTransform.DOLocalMove(Vector3.zero, MoveDuration);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _backgroundFader.Hide(FaderDuration);
            _windowRectTransform.DOScale(Vector3.zero, ScaleDuration);
        }
    }
}