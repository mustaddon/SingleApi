using SingleApi;
using Test;

using var client = new SapiClient("https://localhost:7263/sapi");
var r = await client.Send(new Ping { Message = "abc" });

Console.WriteLine(r?.Message);