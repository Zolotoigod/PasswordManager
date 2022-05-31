/*PasswordService service = new PasswordService(new Restore("restore"), new FileStorage(), new AESKeyService("data/secure"), "storage");
await service.RestorMarks();

Console.WriteLine("Write key");
string? key = Console.ReadLine();

await service.SavePassword(new ExternalData() { Key = key });

var data = await service.ReadPassword(key);

Console.WriteLine(data.Key.ToString() + " - " + data.Password!.ToString());
Console.ReadLine();

await service.SaveMark();*/