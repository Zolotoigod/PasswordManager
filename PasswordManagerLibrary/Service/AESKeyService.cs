using MenagerCore.DTO;
using System.Security.Cryptography;

namespace MenagerCore
{
    public class AESKeyService : IKeyService
    {
        private const string securePath = "secure.dd";

        public AESKeys InitKeys()
        {
            using FileStream stream = new FileStream(securePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            stream.Position = 0;

            if (stream.Length > 0)
            {
                byte[] key = new byte[32];
                byte[] iv = new byte[16];
                using BinaryReader binaryReader = new BinaryReader(stream);

                try
                {
                    binaryReader.Read(key);
                    binaryReader.Read(iv);
                }
                catch (Exception)
                {
                    throw;
                }

                return new AESKeys() { Key = key, IV = iv };
            }

            using BinaryWriter binaryWriter = new BinaryWriter(stream);
            using Aes aes = Aes.Create();

            binaryWriter.Write(aes.Key);
            binaryWriter.Write(aes.IV);

            return new AESKeys() { Key = aes.Key, IV = aes.IV };
        }
    }
}
