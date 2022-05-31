using System.Text;

namespace MenagerCore.Storage
{
    public class FileStorage : IStorage
    {
        public async Task Save(Stream stream, byte[] data)
        {
            stream.Position = stream.Length;
            using BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8);

            await Task.Run(() => writer.Write(data));
        }

        public async Task<byte[]> Read(Stream stream, int position, int length)
        {
            stream.Position = position;
            using BinaryReader reader = new BinaryReader(stream, Encoding.UTF8);

            byte[] bytes = new byte[length];
            await Task.Run(() => reader.Read(bytes));

            return bytes;
        }
    }
}
