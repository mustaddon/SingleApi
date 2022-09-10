using SingleApi.Client;
using Test;

using var mediatr = new SapiClient("https://localhost:7263/mediatr/");
var r = await mediatr.Send(new Ping { Message = "abc" });

Console.WriteLine(r?.Message);

using var sapi = new SapiClient("https://localhost:7263/sapi");
var data = new Dictionary<string, int?[]>
{
    { "key1", new int?[] { 555, null, 777 } }
};
var r2 = await sapi.Send(data, data.GetType());
Console.WriteLine(await sapi.Send<int, int>(111));
Console.WriteLine(await sapi.Send<int[], int[]>(new[] { 11, 22 } ));
Console.WriteLine("done");