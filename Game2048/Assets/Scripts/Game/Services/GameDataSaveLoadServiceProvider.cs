using Core.Services.SaveLoad;
using Game.Configs;
using UnityEngine;

namespace Game.Services
{
    public class GameDataSaveLoadServiceProvider : ISaveLoadServiceProvider<GameData>
    {
        private const string Key = "GameData";
        private readonly ISaveLoadService _saveLoadService;

        public GameDataSaveLoadServiceProvider(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        public void Save(GameData gameData)
        {
            _saveLoadService.Save(Key, gameData);
            PlayerPrefs.Save();
        }

        public GameData Load()
        {
            return _saveLoadService.Load<GameData>(Key);
        }
    }
}