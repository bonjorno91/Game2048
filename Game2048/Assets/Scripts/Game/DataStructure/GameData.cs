using System;
using System.Text;
using Core.DataStructure;
using Core.Services.SaveLoad;

namespace Game.Configs
{
    [Serializable]
    public class GameData : ISaveLoad
    {
        public const string BScore = "BestScore";
        private const string Score = "score";
        private const string Tiles = "tiles";
        private const string SizeX = "sizeX";
        private const string SizeY = "sizeY";
        public GameBoardModel GameBoardModel;
        public bool IsNew { get; private set; } = true;
        public uint BestScore { get; set; } = 0;


        public GameData(GameBoardModel gameBoardModel)
        {
            GameBoardModel = gameBoardModel;
        }
        public GameData(GameBoardModel gameBoardModel, uint bestScore)
        {
            GameBoardModel = gameBoardModel;
            BestScore = bestScore;
        }

        public void OnSave(ISaveLoadService serviceProvider)
        {
            serviceProvider.Save(Score,GameBoardModel.Score);
            serviceProvider.Save(Tiles,ByteArrayToString(GameBoardModel.Tiles));
            serviceProvider.Save(SizeX,GameBoardModel.SizeX);
            serviceProvider.Save(SizeY,GameBoardModel.SizeY);
            serviceProvider.Save(BScore,BestScore);
        }

        public void OnLoad(ISaveLoadService serviceProvider)
        {
            BestScore = serviceProvider.Load<uint>(BScore);
            var score = serviceProvider.Load<uint>(Score);
            var tiles = StringToByteArray(serviceProvider.Load<string>(Tiles));
            var sizeX = serviceProvider.Load<int>(SizeX);
            var sizeY = serviceProvider.Load<int>(SizeY);
            GameBoardModel = GameBoardModel.Load((uint)sizeX,(uint) sizeY, tiles, score);
            IsNew = false;
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