using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Factory.Behaviours
{
    [RequireComponent(typeof(RectTransform))]
    public class MainMenuView : MonoBehaviour
    {
        private const float FaderDuration = 0.5f;
        private const float ScaleDuration = 0.3f;
        private RectTransform _rectTransform;
        private BackgroundFader _backgroundFader;
        [SerializeField] private Button resumeButton;
        public void Initialize(BackgroundFader backgroundFader)
        {
            _backgroundFader = backgroundFader;
            _rectTransform = gameObject.GetComponent<RectTransform>();
            if(resumeButton) resumeButton.onClick.AddListener(Hide);
        }

        public void Show(Vector3 fromPosition)
        {
            _rectTransform.localScale = Vector3.zero;
            gameObject.SetActive(true);
            _backgroundFader.Show(FaderDuration);
            // _rectTransform.DOPos
            _rectTransform.DOScale(Vector3.one, ScaleDuration);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _backgroundFader.Hide(FaderDuration);
            _rectTransform.DOScale(Vector3.zero, ScaleDuration);
        }
    }
}