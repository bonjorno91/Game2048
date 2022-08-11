using System;
using System.Collections.Generic;
using System.Linq;
using Core.Services;
using Game.Configs;
using JetBrains.Annotations;
using Unity.Collections;
using UnityEngine;

namespace Core.DataStructure
{
    [Serializable]
    public class GameBoardModel : ISerializationCallbackReceiver
    {
        private static readonly Vector2Int[] MoveDirections = new[]
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        public event Action<uint> OnScoreUpdated;

        public event Action<int, int, byte> OnTileMoved;

        public event Action<int, int, byte> OnTileCollapsed;

        public uint Score
        {
            get => _score;
            set => _score = value;
        }

        public int SizeY => _sizeY;

        public int SizeX => _sizeX;

        public byte[] Tiles => _tiles;
        [SerializeField] private int _sizeX;
        [SerializeField] private int _sizeY;
        [SerializeField] private byte[] _tiles;
        [SerializeField] private uint _score;
        private GameBoardModel _buffer;

        private bool _isScoreDirty = true;


        // Factory method

        /// <summary>
        /// Create non-square game board.
        /// </summary>
        /// <param name="sizeX">Columns value.</param>
        /// <param name="sizeY">Rows value.</param>
        /// <returns>Non-square game board.</returns>
        public static GameBoardModel GetStartBoard(uint sizeX, uint sizeY)
        {
            var instance = new GameBoardModel(sizeX, sizeY);
            instance.AddStartingTiles();
            instance.InitBuffer();
            return instance;
        }


        // Factory method

        /// <summary>
        /// Create square game board.
        /// </summary>
        /// <param name="sizeXY">Square size.</param>
        /// <returns>Square game board.</returns>
        public static GameBoardModel GetStartBoard(uint sizeXY)
        {
            return GetStartBoard(sizeXY, sizeXY);
        }

        // Factory method
        /// <summary>
        /// Load game board.
        /// </summary>
        /// <param name="sizeX"></param>
        /// <param name="sizeY"></param>
        /// <param name="tiles"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static GameBoardModel Load(uint sizeX, uint sizeY, byte[] tiles, uint score)
        {
            var instance = new GameBoardModel(sizeX, sizeY, tiles);
            instance.Score = score;
            instance.InitBuffer();
            
            return instance;
        }

        private GameBoardModel(uint sizeX, uint sizeY)
        {
            _sizeX = (int) sizeX;
            _sizeY = (int) sizeY;
            _tiles = new byte[sizeX * sizeY];
        }

        private GameBoardModel(uint sizeX, uint sizeY, in byte[] data) : this(sizeX, sizeY)
        {
            if (sizeX * sizeY != data.Length) throw new ArgumentOutOfRangeException();

            data.CopyTo(_tiles, 0);
        }

        /// <summary>
        /// Insert random tile with power value.
        /// </summary>
        /// <param name="power">Power of two.</param>
        /// <returns>Index of tile.</returns>
        public int AddTileRandom(byte power)
        {
            var randomIndex = GetRandomFreeTileIndex();

            if (randomIndex >= 0)
            {
                _tiles[randomIndex] = power;
            }

            return randomIndex;
        }

        /// <summary>
        /// Check for move passability.
        /// </summary>
        /// <returns>TRUE if have next move.</returns>
        public bool CanMove()
        {
            for (int i = 0; i < MoveDirections.Length; i++)
            {
                if (CanMoveDirection(MoveDirections[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private void InitBuffer()
        {
            _buffer = new GameBoardModel((uint) _sizeX, (uint) _sizeY);
        }

        private void AddStartingTiles()
        {
            AddTileRandom(1);
            AddTileRandom((byte) UnityEngine.Random.Range(1, 3));
        }

        private GameBoardModel GetLoadedBuffer()
        {
            _buffer.LoadBuffer(this);
            return _buffer;
        }

        private GameBoardModel LoadBuffer(GameBoardModel source)
        {
            source._tiles.CopyTo(_tiles, 0);
            return this;
        }

        public bool HandleMove(Vector2Int direction)
        {
            if (!CanMoveDirection(direction))
                return false;

            MoveInternal(direction);
            OnUpdateScore();

            return true;
        }

        private bool CanMoveDirection(Vector2Int direction)
        {
            return !GetLoadedBuffer().MoveInternal(direction).IsEqual(this);
        }

        private int GetRandomFreeTileIndex()
        {
            var list = new List<int>(_tiles.Length);

            for (int i = 0; i < _tiles.Length; i++)
            {
                if (_tiles[i] == 0)
                {
                    list.Add(i);
                }
            }

            if (list.Count > 0)
            {
                if (list.Count == 1)
                {
                    return list[0];
                }

                return list[UnityEngine.Random.Range(0, list.Count)];
            }

            return -1;
        }

        private GameBoardModel MoveInternal(Vector2Int direction)
        {
            if (direction.x != 0)
            {
                AdditionHorizontal(direction.x);
                CollapseHorizontal(direction.x);
            }
            else
            {
                AdditionVertical(direction.y);
                CollapseVertical(direction.y);
            }

            return this;
        }

        public bool IsEqual([NotNull] GameBoardModel other)
        {
            for (int i = 0; i < _tiles.Length; i++)
            {
                if (_tiles[i] != other._tiles[i]) return false;
            }

            return true;
        }

        private void IncreaseScore(uint score)
        {
            _isScoreDirty = true;
            Score += score;
        }

        private void OnUpdateScore()
        {
            if (_isScoreDirty)
            {
                OnScoreUpdated?.Invoke(Score);
                _isScoreDirty = false;
            }
        }

        private void AdditionHorizontal(int directionX)
        {
            directionX = -directionX;
            var startIndex = ((_sizeX + directionX) >> 1) + ((_sizeX >> 1) * -directionX);

            for (int y = 0, previous = -1; y < _sizeY; y++, previous = -1)
            {
                for (int x = startIndex, column = 0; column < _sizeX; x += directionX, column++)
                {
                    int current = GetIndexByPosition(x, y);

                    // current is number
                    if (_tiles[current] > 0)
                    {
                        // looking for duplicate
                        if (previous >= 0)
                        {
                            // current is duplicate
                            if (_tiles[current] == _tiles[previous])
                            {
                                // increase current value and go looking next index
                                _tiles[current] += 1;
                                _tiles[previous] = 0;
                                OnTileCollapsed?.Invoke(previous, current, _tiles[current]);
                                IncreaseScore((uint) Mathf.Pow((int) _tiles[current], 2));
                                previous = -1;
                                continue;
                            }
                        }

                        // begin looking for duplicate in next
                        previous = current;
                    }
                }
            }
        }

        private void AdditionVertical(int directionY)
        {
            directionY = -directionY;
            var startIndex = ((_sizeY + directionY) >> 1) + ((_sizeY >> 1) * -directionY);

            for (int x = 0, previous = -1; x < _sizeX; x++, previous = -1)
            {
                for (int y = startIndex, row = 0; row < _sizeY; y += directionY, row++)
                {
                    int current = GetIndexByPosition(x, y);

                    // current is number
                    if (_tiles[current] > 0)
                    {
                        // looking for duplicate
                        if (previous >= 0)
                        {
                            // current is duplicate
                            if (_tiles[current] == _tiles[previous])
                            {
                                // increase prev value and go looking next index
                                _tiles[current] += 1;
                                _tiles[previous] = 0;
                                OnTileCollapsed?.Invoke(previous, current, _tiles[current]);
                                IncreaseScore((uint) Mathf.Pow((int) _tiles[current], 2));
                                previous = -1;
                                continue;
                            }
                        }

                        // begin looking for duplicate in next
                        previous = current;
                    }
                }
            }
        }

        private void CollapseHorizontal(int directionX)
        {
            // invert direction
            directionX = -directionX;
            var startIndex = ((_sizeX + directionX) >> 1) + (_sizeX >> 1) * -directionX;

            for (int y = 0, column = 0, searchX = startIndex; y < _sizeY; y++, column = 0, searchX = startIndex)
            {
                for (int x = startIndex; column < _sizeX; x += directionX)
                {
                    int current = GetIndexByPosition(x, y);

                    // step on free tile
                    if (_tiles[current] == 0)
                    {
                        // start searching occupied tile
                        for (; column < _sizeY; column++, searchX += directionX)
                        {
                            var searchIndex = GetIndexByPosition(searchX, y);

                            // tile is occupied
                            if (_tiles[searchIndex] != 0)
                            {
                                _tiles[current] = _tiles[searchIndex];
                                _tiles[searchIndex] = 0;
                                OnTileMoved?.Invoke(searchIndex, current, _tiles[current]);
                                break;
                            }
                        }
                    }
                    // step on occupied tile
                    else
                    {
                        column++;
                        searchX += directionX;
                    }
                }
            }
        }

        private void CollapseVertical(int directionY)
        {
            // invert direction
            directionY = -directionY;
            var startIndex = ((_sizeY + directionY) >> 1) + ((_sizeY >> 1) * -directionY);

            for (int x = 0, row = 0, searchY = startIndex;
                 x < _sizeX;
                 x++, row = 0, searchY = startIndex)
            {
                for (int y = startIndex; row < _sizeY; y += directionY)
                {
                    int current = GetIndexByPosition(x, y);

                    // step on free tile
                    if (_tiles[current] == 0)
                    {
                        // start searching occupied tile
                        for (; row < _sizeY; row++, searchY += directionY)
                        {
                            var searchIndex = GetIndexByPosition(x, searchY);

                            // tile is occupied
                            if (_tiles[searchIndex] != 0)
                            {
                                _tiles[current] = _tiles[searchIndex];
                                _tiles[searchIndex] = 0;
                                OnTileMoved?.Invoke(searchIndex, current, _tiles[current]);
                                break;
                            }
                        }
                    }
                    // step on occupied tile
                    else
                    {
                        row++;
                        searchY += directionY;
                    }
                }
            }
        }

        private int GetPositionY(int index)
        {
            return index / _sizeX;
        }

        private int GetPositionX(int index)
        {
            return index % _sizeX;
        }

        public Vector2Int GetPositionByIndex(int index)
        {
            return new Vector2Int(index % _sizeX, index / _sizeX);
        }

        private int GetIndexByPosition(Vector2Int position)
        {
            return position.y * _sizeX + position.x;
        }

        private int GetIndexByPosition(int x, int y)
        {
            return y * _sizeX + x;
        }

        private bool IsInBoundaries(Vector2Int position)
        {
            return position.x >= 0 && position.x < _sizeX && position.y >= 0 && position.y < _sizeY;
        }

        private bool IsInBoundaries(int x, int y)
        {
            return x >= 0 && x < _sizeX && y >= 0 && y < _sizeY;
        }

        private bool IsInBoundaries(int index)
        {
            return index >= 0 && index < _tiles.Length && IsInBoundaries(GetPositionByIndex(index));
        }

        public void OnBeforeSerialize()
        {
            
        }

        public void OnAfterDeserialize()
        {
            InitBuffer();
        }
    }
}