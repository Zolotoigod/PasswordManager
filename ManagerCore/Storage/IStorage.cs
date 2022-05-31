namespace MenagerCore.Storage
{
    public interface IStorage
    {
        Task<byte[]> Read(Stream stream, int position, int length);
        Task Save(Stream stream, byte[] data);
    }
}