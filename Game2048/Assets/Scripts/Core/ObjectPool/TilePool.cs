using System.Collections.Generic;
using System.Linq;
using Game.Behaviours;
using UnityEngine;

namespace Core.ObjectPool
{
    public class TilePool : IPool<GameBoardTileView, byte>
    {
        private readonly GameBoardTileView _instance;
        private readonly Transform _parent;
        private int _size;
        private Stack<GameBoardTileView> _pool;

        public TilePool(GameBoardTileView instance, uint size, Transform parent)
        {
            _instance = instance;
            _parent = parent;
            _size = (int) size;
            _pool = new Stack<GameBoardTileView>(_size);

            for (int i = 0; i < _size; i++)
            {
                AddInstance();
            }
        }

        public void Release(GameBoardTileView releaseObject)
        {
            releaseObject.OnRelease();
            releaseObject.gameObject.SetActive(false);
            _pool.Push(releaseObject);
        }

        public GameBoardTileView Get(Vector3 position, Quaternion quaternion, byte payload)
        {
            if (_pool.Count == 0)
            {
                AddInstance();
            }

            _pool.Peek().OnGetFromPool(position, quaternion, payload);
            return _pool.Pop();
        }

        private int conter = 0;
        private void AddInstance()
        {
            var poolObject = Object.Instantiate(_instance, _parent);
            poolObject.gameObject.name = $"Tile {conter++}";
            poolObject.InitPool(this);
            poolObject.OnRelease();
            _pool.Push(poolObject);
        }
    }
}