namespace Game.Configs
{
    public interface ISaveLoadServiceProvider<TData> where TData : ISaveLoad
    {
        void Save(TData data);
        TData Load();
    }
}