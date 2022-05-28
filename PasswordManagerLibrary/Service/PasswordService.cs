using MenagerCore.DTO;
using MenagerCore.Storage;
using System.Text;

namespace MenagerCore
{
    public class PasswordService
    {
        private readonly IRestore restore;
        private readonly IStorage passwordStorage;
        private readonly string storePath;
        private readonly AESKeys aESKeys; 
        private Dictionary<string, ReadLocation>? marks;

        public PasswordService(IRestore restore, IStorage passwordStorage, IKeyService keyService, string path)
        {
            this.restore = restore;
            this.passwordStorage = passwordStorage;
            aESKeys = keyService.InitKeys();
            storePath = path;
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

            if (inputData.Comment  is null)
            {
                inputData.Comment = "NoComment";
            }

            var data = new Data()
            {
                Key = inputData.Key,
                Password = Incrypt(inputData.Password),
                Comment = inputData.Comment
            };

            Console.WriteLine(string.Join("|", data.Password));

            var bytes = PrepareToWrite(data);

            using FileStream streamStorage = new FileStream(storePath, FileMode.OpenOrCreate, FileAccess.Write);
            marks!.Add(inputData.Key,new ReadLocation() { Position = (int)streamStorage.Length, Length = bytes.Length });

            await Task.Run(() => passwordStorage.Save(streamStorage, bytes));
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

        public async Task SaveMark()
        {
            await restore.Save(marks!);
        }

        private byte[] Incrypt(string password)
        {
            return Core.Incrypted(password, aESKeys.Key, aESKeys.IV);
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


            Console.WriteLine(string.Join("|", result));

            return Core.Decrypted(result, aESKeys.Key, aESKeys.IV);
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
            var decryptPass = Decrypt(dataArray[1]);


            return new ExternalData()
            {
                Key = dataArray[0],
                Password = decryptPass,
                Comment = dataArray[2],
            };
        }
        
        private async Task<byte[]> ReadByte(string key)
        {
            using FileStream streamStorage = new FileStream(storePath, FileMode.OpenOrCreate, FileAccess.Read);
            return await passwordStorage.Read(streamStorage, marks![key].Position, marks[key].Length);
        }
    }
}
