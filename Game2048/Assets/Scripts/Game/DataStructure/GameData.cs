using System;
using System.Text;
using Core.DataStructure;
using Core.Services.SaveLoad;
using UnityEngine;

namespace Game.Configs
{
    [Serializable]
    public class GameData : ISerializationCallbackReceiver, ISaveLoad
    {
        public const string BScore = "BestScore";
        private const string Score = "score";
        private const string Tiles = "tiles";
        private const string SizeX = "sizeX";
        private const string SizeY = "sizeY";
        [SerializeField] private uint _bestScore;
        public GameBoardModel GameBoardModel;

        private GameData()
        {
        }

        public bool IsInitialized { get; set; }

        public uint BestScore
        {
            get => _bestScore;
            set => _bestScore = value;
        }

        public void OnSave(ISaveLoadService serviceProvider)
        {
            serviceProvider.Save(Score, GameBoardModel.Score);
            serviceProvider.Save(Tiles, ByteArrayToString(GameBoardModel.Tiles));
            serviceProvider.Save(SizeX, GameBoardModel.SizeX);
            serviceProvider.Save(SizeY, GameBoardModel.SizeY);
            serviceProvider.Save(BScore, BestScore);
        }

        public void OnLoad(ISaveLoadService serviceProvider)
        {
            var score = serviceProvider.Load<uint>(Score);
            var tiles = StringToByteArray(serviceProvider.Load<string>(Tiles));
            var sizeX = serviceProvider.Load<int>(SizeX);
            var sizeY = serviceProvider.Load<int>(SizeY);
            GameBoardModel = GameBoardModel.Load((uint) sizeX, (uint) sizeY, tiles, score);
        }

        public void OnBeforeSerialize()
        {
        }

        public void OnAfterDeserialize()
        {
            IsInitialized = false;
        }

        public void NewGame(uint size)
        {
            if (GameBoardModel.Score > _bestScore)
                _bestScore = GameBoardModel.Score;

            GameBoardModel = GameBoardModel.GetStartBoard(size);
            IsInitialized = false;
        }

        public static GameData NewGameSession(uint boardSize)
        {
            var gameData = new GameData();
            gameData.GameBoardModel = GameBoardModel.GetStartBoard(boardSize);
            gameData._bestScore = 0;
            return gameData;
        }

        private string ByteArrayToString(byte[] array)
        {
            return Encoding.Default.GetString(array);
        }

        private byte[] StringToByteArray(string stringArray)
        {
            return Encoding.Default.GetBytes(stringArray);
        }
    }
}