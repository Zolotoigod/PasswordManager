using MenagerCore.DTO;
using System.Text.Json;

namespace MenagerCore.Storage
{
    public class Restore : IRestore
    {
        private readonly string sysPath;

        public Restore(string sysPath = "data/restore")
        {
            this.sysPath = sysPath;
        }

        public async Task Save(Dictionary<string, ReadLocation> manageInfo)
        {
            using FileStream fileStream = new FileStream(sysPath, FileMode.Open, FileAccess.Write);

            await JsonSerializer.SerializeAsync(fileStream,
                manageInfo,
                typeof(Dictionary<string, ReadLocation>),
                new JsonSerializerOptions { AllowTrailingCommas = true });
        }

        public async Task<Dictionary<string, ReadLocation>> Read()
        {
            using FileStream fileStream = new FileStream(sysPath, FileMode.OpenOrCreate, FileAccess.Read);
            if (fileStream.Length > 0)
            {
                return await JsonSerializer.DeserializeAsync<Dictionary<string, ReadLocation>>(fileStream, new JsonSerializerOptions { AllowTrailingCommas = true })
                ?? new Dictionary<string, ReadLocation>();
            }
            
            return new Dictionary<string, ReadLocation>();
        }
    }
}
