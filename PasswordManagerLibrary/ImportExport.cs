using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MenagerCore
{
    public static class ImportExport
    {
        public static async Task WriteToFile(string key, byte[] password)
        {
            using FileStream fs = new FileStream("data", FileMode.OpenOrCreate, FileAccess.Write);
            fs.Position = fs.Length;

            using StreamWriter writer = new StreamWriter(fs, Encoding.UTF8);

            StringBuilder sb = new StringBuilder();
            sb.Append(key + "-");
            sb.Append(string.Join("|", password));

            await writer.WriteLineAsync(sb);
        }

        public static async Task<byte[]> ReadFromFile(string key)
        {
            using FileStream fs = new FileStream("data", FileMode.Open, FileAccess.Read);
            fs.Position = 0;

            using StreamReader reader = new StreamReader(fs, Encoding.UTF8);

            string store = await reader.ReadToEndAsync();

            string[] data = store.Split(new string[] {Environment.NewLine, "-"}, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < data.Length; i++)
            {
                if(data[i].Equals(key))
                {
                    string[] array = data[i+1].Split("|");
                    byte[] result = new byte[array.Length];
                    int j = 0;
                    foreach (string b in array)
                    {
                        result[j] = Convert.ToByte(b);
                        j++;
                    }

                    return result;
                }
            }

            return Array.Empty<byte>();
        }
    }
}
