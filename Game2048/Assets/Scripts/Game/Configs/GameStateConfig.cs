using System;
using Core.DataStructure;
using UnityEngine;

namespace Game
{
    [Serializable]
    public class GameStateConfig
    {
        public GameBoardModel Field => gameBoardModel;
        [SerializeField] private GameBoardModel gameBoardModel;
        
        public GameStateConfig(uint size)
        {
            gameBoardModel = GameBoardModel.GetStartBoard(size);
        }
    }
}