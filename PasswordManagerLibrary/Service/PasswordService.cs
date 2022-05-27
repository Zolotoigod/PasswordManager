using MenagerCore.DTO;
using MenagerCore.Storage;
using System.Security.Cryptography;
using System.Text;

namespace MenagerCore
{
    public class PasswordService
    {
        private readonly IRestore restore;
        private readonly IStorage passwordStorage;
        private Dictionary<string, (int position, int length)>? marks;

        public PasswordService(IRestore restore, IStorage passwordStorage)
        {
            this.restore = restore;
            this.passwordStorage = passwordStorage;
        }

        public async Task SavePassword(ExternalData inputData)
        {
            if (inputData is null)
            {
                throw new ArgumentNullException(nameof(inputData));
            }

            if (inputData.Password is null)
            {
                inputData.Password = Core.GeneratePass();
            }

            var data = new Data()
            {
                Key = inputData.Key,
                Password = Incrypt(inputData.Password),
                Comment = inputData.Comment
            };

            await Task.Run(() => passwordStorage.Save(PrepareToWrite(data)));
        }

        public async Task<ExternalData> ReadPassword(string key)
        {
            if(marks!.ContainsKey(key))
            {
                return await BuildData(key);
            }

            throw new InvalidOperationException("Key not found!");
        }

        public async Task RestorMarks()
        {
            marks = await restore.Read();
        }

        private byte[] Incrypt(string password)
        {
            using Aes newAes = Aes.Create();

            return Core.Incrypted(password, newAes.Key, newAes.IV);
        }

        private string Decrypt(string bytePassword)
        {
            var array = bytePassword.Split("|");
            byte[] result = new byte[array.Length];
            int j = 0;
            foreach (string b in array)
            {
                result[j] = Convert.ToByte(b);
                j++;
            }

            using Aes newAes = Aes.Create();

            return Core.Decrypted(result, newAes.Key, newAes.IV);
        }

        private byte[] PrepareToWrite(Data data)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(data.Key + "-");
            sb.Append(string.Join("|", data.Password));
            sb.Append("-" + data.Comment);

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        private async Task<ExternalData> BuildData(string key)
        {
            var bytes = await ReadByte(key);
            string data = Encoding.UTF8.GetString(bytes.AsSpan());

            string[] dataArray = data.Split(new string[] { Environment.NewLine, "-" }, StringSplitOptions.RemoveEmptyEntries);

            return new ExternalData()
            {
                Key = dataArray[0],
                Password = Decrypt(dataArray[1]),
                Comment = dataArray[2],
            };
        }
        
        private async Task<byte[]> ReadByte(string key)
        {
            return await passwordStorage.Read(marks![key].position, marks[key].length);
        }
    }
}
