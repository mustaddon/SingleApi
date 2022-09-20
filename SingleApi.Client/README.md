# SingleApi Client [![NuGet version](https://badge.fury.io/nu/SingleApi.Client.svg)](http://badge.fury.io/nu/SingleApi.Client)
.NET SingleApi client


## Example: Usage
*.NET CLI*
```
dotnet new console --name "SapiClientExample"
cd SapiClientExample
dotnet add package SingleApi.Client
```

*Program.cs:*
```C#
using SingleApi.Client;

// create client
using var sapi = new SapiClient("https://localhost:7263/sapi");

// send request
var response = await sapi.Send(new Ping { Message = "TEST" });

Console.WriteLine(response?.Message);
```

*Console output:*
```
TEST PONG
```

[Example project...](https://github.com/mustaddon/SingleApi/tree/main/Examples/Example.Client)
