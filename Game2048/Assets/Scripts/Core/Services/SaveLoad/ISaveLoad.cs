namespace Core.Services.SaveLoad
{
    public interface ISaveLoad
    {
        void OnSave(ISaveLoadService serviceProvider);
        void OnLoad(ISaveLoadService serviceProvider);
    }
}