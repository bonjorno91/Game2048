namespace Game.Configs
{
    public interface ISaveLoad
    {
        void OnSave(ISaveLoadService serviceProvider);
        void OnLoad(ISaveLoadService serviceProvider);
    }
}