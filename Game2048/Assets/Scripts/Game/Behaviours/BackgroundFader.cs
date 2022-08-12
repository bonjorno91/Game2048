using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviours
{
    [RequireComponent(typeof(RectTransform))]
    public class BackgroundFader : MonoBehaviour
    {
        private Image _image;
        private Color _originalColor;
        private RectTransform _rectTransform;
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
            _image.DOFade(_originalColor.a, duration);
        }

        public void Hide(float duration)
        {
            _image.DOFade(0, duration).OnComplete(Disable);
        }

        private void Disable()
        {
            gameObject.SetActive(false);
        }

        private void SetTransparent()
        {
            _image.color = _transparentColor;
        }
    }
}