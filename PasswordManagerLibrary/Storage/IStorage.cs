namespace MenagerCore.Storage
{
    public interface IStorage
    {
        Task<byte[]> Read(int position, int length);
        Task Save(byte[] data);
    }
}