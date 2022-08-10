using System;
using System.Collections;
using Core.ObjectPool;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviours
{
    [RequireComponent(typeof(RectTransform))]
    public class GameBoardTileView : MonoBehaviour, IPoolingTile
    {
        private const int minDistance = 1;

        public uint Power
        {
            get => _power;
            set
            {
                _power = value;
                _textMeshPro.text = ((uint) Mathf.Pow(2, _power)).ToString();
                _textMeshPro.color = _power < 3 ? Color.grey : Color.white;
                _image.color = Color.HSVToRGB(0.06f + (0.002f * (_power / 8f)), _power / 8f, 0.9f);
            }
        }

        public RectTransform RectTransform => _rectTransform;

        private uint _power;
        private RectTransform _rectTransform;
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private Image _image;
        private TilePool _pool;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public void PlayInitAnimation()
        {
            var color = _image.color;
            color.a = 0;
            _image.color = color;
            _image.DOFade(1, 0.05f);
            _rectTransform.DOScale(1, 0.05f);
        }

        public void PlayPowerUpdateAnimation()
        {
            _rectTransform.DOShakeScale(0.15f,0.5f);
        }
        
        public void MoveTo(Transform position)
        {
            gameObject.transform.DOMove(position.position, 0.05f);
        }
        
        public void InitPool(TilePool pool)
        {
            _pool = pool;
            useGUILayout = false;
        }

        public void OnGetFromPool(Vector3 position, Quaternion quaternion, byte payload)
        {
            gameObject.transform.position = position;
            Power = payload;
            gameObject.SetActive(true);
        }

        public void OnRelease()
        {
            gameObject.SetActive(false);
        }

        public void DestroyAndReturnPool()
        {
            _pool.Release(this);
        }
    }
}