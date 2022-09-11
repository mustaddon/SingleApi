using Example;
using SingleApi.Client;

// create client
using var sapi = new SapiClient("https://localhost:7263/sapi");

// send request
var response = await sapi.Send(new Ping { Message = "TEST" });

Console.WriteLine(response?.Message);