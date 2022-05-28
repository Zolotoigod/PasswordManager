using MenagerCore.DTO;

namespace MenagerCore.Storage
{
    public interface IRestore
    {
        Task<Dictionary<string, ReadLocation>> Read();
        Task Save(Dictionary<string, ReadLocation> manageInfo);
    }
}