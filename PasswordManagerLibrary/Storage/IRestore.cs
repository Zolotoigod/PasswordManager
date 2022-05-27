namespace MenagerCore.Storage
{
    public interface IRestore
    {
        Task<Dictionary<string, (int position, int length)>> Read();
        Task Save(Dictionary<string, (int position, int length)> manageInfo);
    }
}