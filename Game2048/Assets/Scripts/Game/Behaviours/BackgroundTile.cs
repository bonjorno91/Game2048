using Core.ObjectPool;
using UnityEngine;

namespace Game.Behaviours
{
    [RequireComponent(typeof(RectTransform))]
    public class BackgroundTile : MonoBehaviourPoolable<BackgroundTile>
    {
        [SerializeField] private RectTransform _rectTransform;
        public int Index = -1;
        public RectTransform RectTransform => _rectTransform;

        public override void OnPoolableInitialize(IMonoPoolInitializable<BackgroundTile> pool)
        {
        }

        public override void OnReturnToPool()
        {
            gameObject.SetActive(false);
        }

        public override BackgroundTile OnGetFromPool(Vector3 worldPosition, Quaternion rotation)
        {
            gameObject.SetActive(true);

            return this;
        }
    }
}