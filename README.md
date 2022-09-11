# SingleApi [![NuGet version](https://badge.fury.io/nu/SingleApi.svg)](http://badge.fury.io/nu/SingleApi)
Single/generic WebApi handler (mediator ready)


## Features
* Mediator ready
* Generics support


## Example 1: SingleApi with MediatR
.NET CLI
```
dotnet new web --name "SingleApiExample"
cd SingleApiExample
dotnet add package SingleApi
dotnet add package MediatR.Extensions.Microsoft.DependencyInjection
```

Change Program.cs
```C#
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.MapSingleApi("sapi", 
    // invoke the MediatR
    x => x.ServiceProvider.GetRequiredService<IMediator>().Send(x.Data, x.CancellationToken),
    // assemblies for type resolving
    Assembly.GetExecutingAssembly()); 

app.Run();
```

[Example project...](https://github.com/mustaddon/SingleApi/tree/main/Examples/Example.MediatR)


## Example 2: Request/Response
```
// Request
GET /sapi/Ping?data={"Message":"TEST"}

or

POST /sapi/Ping
{"Message":"TEST"}


// Response
{"message":"TEST PONG"}
```


## Example 3: Generic types
```C#
app.MapSingleApi("sapi", 
    // for simplicity, return the received data
    x => Task.FromResult(x.Data), 
    // existing generic types will suffice for this example
    typeof(List<>).Assembly, typeof(int).Assembly); 
```

```
// Request #1: equivalent of List<String>
POST /sapi/List(String)
["text1","text2","text3"]


// Request #2: equivalent of Dictionary<string,int?[]>
POST /sapi/Dictionary(String,Array(Nullable(Int32)))
{"key1":[555,null,777]}
```


## Example 4: SingleApi.Client
.NET CLI
```
dotnet new console --name "SapiClientExample"
cd SapiClientExample
dotnet add package SingleApi.Client
```

Program.cs:
```C#
using SingleApi.Client;

// create client
using var sapi = new SapiClient("https://localhost:7263/sapi");

// send request
var response = await sapi.Send(new Ping { Message = "TEST" });

Console.WriteLine(response?.Message);


//// Console output:
// TEST PONG
```

[Example project...](https://github.com/mustaddon/SingleApi/tree/main/Examples/Example.Client)
