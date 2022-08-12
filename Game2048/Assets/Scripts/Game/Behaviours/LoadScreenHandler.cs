using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviours
{
    [RequireComponent(typeof(Canvas))]
    [RequireComponent(typeof(CanvasGroup))]
    public class LoadScreenHandler : MonoBehaviour
    {
        [SerializeField] private Slider _slider;

        public void Show(float duration)
        {
            var canvasGroup = GetComponent<CanvasGroup>();
            gameObject.SetActive(true);
            var tween = DOTween.Sequence();
            tween.Append(_slider.DOValue(1, duration));
            tween.Append(canvasGroup.DOFade(0, duration / 2));
            tween.AppendCallback(() => { gameObject.SetActive(false); });
            tween.Play();
        }
    }
}