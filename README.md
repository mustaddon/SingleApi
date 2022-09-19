# SingleApi [![NuGet version](https://badge.fury.io/nu/SingleApi.svg)](http://badge.fury.io/nu/SingleApi)
Single/generic WebApi endpoint for mediators


## Features
* Ready for mediators
* Generics requests
* File streams


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
{"Message":"TEST PONG"}
```


## Example 3: Generics requests
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


## Example 4: .NET client
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


## Example 5: File upload
Create RequestHandler
```C#
using MediatR;
using SingleApi;
namespace Example;

public class FileUpload : IRequest<string>, ISapiFile
{
    public Stream Content { get; set; } = Stream.Null;
    public string? Type { get; set; }
    public string? Name { get; set; }
}

public class FileUploadHandler : IRequestHandler<FileUpload, string>
{
    public async Task<string> Handle(FileUpload request, CancellationToken cancellationToken)
    {
        var filePath = Path.GetFullPath(request.Name);
        using var fileStream = File.Create(filePath);
        await request.Content.CopyToAsync(fileStream, cancellationToken);
        return filePath;
    }
}
```

Sending a file in JavaScript
```js
let file = document.getElementById('my-input').files[0];

let response = await fetch('/sapi/FileUpload', {
    method: 'POST',
    headers: {
        'content-type': file.type || 'application/octet-stream',
        'content-disposition': `attachment; filename*=utf-8''${encodeURIComponent(file.name)}`,
    },
    body: file,
});

console.log('result:', await response.json());
```

[Example project...](https://github.com/mustaddon/SingleApi/tree/main/Examples/Example.MediatR)