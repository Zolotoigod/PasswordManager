using System.Text.Json;

namespace MenagerCore.Storage
{
    public class Restore : IRestore
    {
        private readonly string sysPath;

        public Restore(string sysPath)
        {
            this.sysPath = sysPath;
        }

        public async Task Save(Dictionary<string, (int position, int length)> manageInfo)
        {
            using FileStream fileStream = new FileStream(sysPath, FileMode.Open, FileAccess.Write);

            await JsonSerializer.SerializeAsync(fileStream,
                manageInfo,
                typeof(Dictionary<string, (int position, int length)>),
                new JsonSerializerOptions { AllowTrailingCommas = true });
        }

        public async Task<Dictionary<string, (int position, int length)>> Read()
        {
            using FileStream fileStream = new FileStream(sysPath, FileMode.Open, FileAccess.Write);

            return await JsonSerializer.DeserializeAsync<Dictionary<string, (int position, int length)>>(fileStream, new JsonSerializerOptions { AllowTrailingCommas = true })
                ?? new Dictionary<string, (int position, int length)>();
        }
    }
}
