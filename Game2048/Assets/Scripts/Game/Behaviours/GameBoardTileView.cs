using Core.ObjectPool;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Behaviours
{
    [RequireComponent(typeof(RectTransform))]
    public class GameBoardTileView : MonoBehaviourPoolablePayload<GameBoardTileView, byte>
    {
        [SerializeField] private TextMeshProUGUI _textMeshPro;
        [SerializeField] private Image _image;
        private IMonoPoolInitializable<GameBoardTileView> _pool;
        private uint _power;

        public uint Power
        {
            get => _power;
            set
            {
                _power = value;
                _textMeshPro.text = ((uint) Mathf.Pow(2, _power)).ToString();
                _textMeshPro.color = _power < 3 ? Color.grey : Color.white;
                _image.color = Color.HSVToRGB(0.06f + 0.002f * (_power / 8f), _power / 8f, 0.9f);
            }
        }

        public RectTransform RectTransform { get; private set; }

        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }

        public void PlayInitAnimation()
        {
            var color = _image.color;
            color.a = 0;
            _image.color = color;
            _image.DOFade(1, 0.05f);
            RectTransform.DOScale(1, 0.05f);
        }

        public void PlayPowerUpdateAnimation()
        {
            RectTransform.DOShakeScale(0.15f, 0.5f);
        }

        public void MoveTo(Transform position)
        {
            gameObject.transform.DOMove(position.position, 0.05f);
        }

        public void DestroyAndReturnPool()
        {
            _pool.ReturnToPool(this);
        }

        public override void OnPoolableInitialize(IMonoPoolInitializable<GameBoardTileView> pool)
        {
            _pool = pool;
            useGUILayout = false;
        }

        public override void OnReturnToPool()
        {
            Power = 1;
            gameObject.SetActive(false);
        }

        public override GameBoardTileView OnGetFromPool(Vector3 worldPosition, Quaternion rotation, byte data)
        {
            gameObject.transform.position = worldPosition;
            Power = data;
            gameObject.SetActive(true);

            return this;
        }

        public void PlayGameOver()
        {
            gameObject.transform.DOLocalJump(Vector3.right * Random.Range(-500, 500) + Vector3.down * 800, 800, 1, 3f);
        }
    }
}