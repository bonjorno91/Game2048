using UnityEngine;

namespace Game.Configs
{
    public class SaveLoadService : ISaveLoadService
    {
        public void Save<T>(string key, T data)
        {
            PlayerPrefs.SetString(key,JsonUtility.ToJson(data));
        }

        public T Load<T>(string key)
        {
            return JsonUtility.FromJson<T>(PlayerPrefs.GetString(key));
        }
    }
}