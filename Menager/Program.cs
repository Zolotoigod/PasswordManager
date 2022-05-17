
using MenagerCore;
using System.Security.Cryptography;

var key = Console.ReadLine();
using Aes newAes = Aes.Create();
await ImportExport.WriteToFile(key, Core.Incrypted(Core.GeneratePass(), newAes.Key, newAes.IV));

Console.WriteLine((Core.Decrypted(await ImportExport.ReadFromFile(key), newAes.Key, newAes.IV)));
