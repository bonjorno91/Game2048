namespace Core.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void Save<T>(string key, T data);
        T Load<T>(string key);
    }
}