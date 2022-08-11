using Core.DataStructure;
using UnityEngine;

namespace Game.Configs
{
    public class GameDataSaveLoadServiceProvider : ISaveLoadServiceProvider<GameData>
    {
        private const string Key = "GameData";
        private readonly ISaveLoadService _saveLoadService;
        private readonly uint _defaultBoardSize;

        public GameDataSaveLoadServiceProvider(ISaveLoadService saveLoadService, uint defaultBoardSize)
        {
            _saveLoadService = saveLoadService;
            _defaultBoardSize = defaultBoardSize;
        }

        public void Save(GameData gameData)
        {
            _saveLoadService.Save(Key, gameData);
            // gameData.OnSave(_saveLoadService);
            PlayerPrefs.Save();
        }

        public GameData Load()
        {
            var gameData = _saveLoadService.Load<GameData>(Key);
            
            if (gameData == null || gameData.GameBoardModel == null || gameData.GameBoardModel.Tiles == null)
            {
                gameData = new GameData(GameBoardModel.GetStartBoard(_defaultBoardSize));
                Save(gameData);
            }
            else
            {
                // gameData.OnLoad(_saveLoadService);
            }

            return gameData;
        }
    }
}