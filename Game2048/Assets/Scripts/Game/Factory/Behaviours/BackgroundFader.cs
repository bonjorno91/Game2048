using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Factory.Behaviours
{
    [RequireComponent(typeof(RectTransform))]
    public class BackgroundFader : MonoBehaviour
    {
        private RectTransform _rectTransform;
        private Image _image;
        private Color _originalColor;
        private Color _transparentColor;

        public void Initialize()
        {
            _rectTransform = gameObject.GetComponent<RectTransform>();
            _image = gameObject.GetComponent<Image>();
            _originalColor = _image.color;
            _transparentColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, 0);
            SetTransparent();
        }

        public void Show(float duration)
        {
            SetTransparent();
            gameObject.SetActive(true);
            _image.DOFade(1, duration);
        }

        public void Hide(float duration)
        {
            _image.DOFade(0, duration).OnComplete(Disable);
        }

        private void Disable()
        {
            gameObject.SetActive(true);
        }
        
        private void SetTransparent()
        {
            _image.color = _transparentColor;
        }
    }
}