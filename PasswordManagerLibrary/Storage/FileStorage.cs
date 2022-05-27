using System.Text;

namespace MenagerCore.Storage
{
    public class FileStorage : IStorage
    {
        private readonly string currentPath;

        public FileStorage(string path)
        {
            currentPath = path;
        }

        public async Task Save(byte[] data)
        {
            using FileStream fileStream = new FileStream(currentPath, FileMode.OpenOrCreate, FileAccess.Write);
            fileStream.Position = fileStream.Length;

            using BinaryWriter writer = new BinaryWriter(fileStream, Encoding.UTF8);

            await Task.Run(() => writer.Write(data));
        }

        public async Task<byte[]> Read(int position, int length)
        {
            if (!File.Exists(currentPath))
            {
                throw new FileNotFoundException(currentPath);
            }

            using FileStream fileStream = new FileStream(currentPath, FileMode.Open, FileAccess.Read);
            using BinaryReader reader = new BinaryReader(fileStream, Encoding.UTF8);

            byte[] bytes = new byte[length];
            await Task.Run(() => reader.Read(bytes, position, length));

            return bytes;
        }
    }
}
