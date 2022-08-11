namespace Core.Services.SaveLoad
{
    public interface ISaveLoadServiceProvider<TData> where TData : ISaveLoad
    {
        void Save(TData data);
        TData Load();
    }
}