using Example;
using SingleApi.Client;
using System.Text;


// create client
using var sapi = new SapiClient("https://localhost:7263/sapi");


// ping request
var pingResponse = await sapi.Send(new Ping { Message = "TEST" });
Console.WriteLine($"ping result: {pingResponse?.Message}");


// file upload request
var uploadResponse = await sapi.Send(new FileUpload
{
    Content = new MemoryStream(Encoding.UTF8.GetBytes("text text text")),
    Type = "text/plain",
    Name = "example.txt",
});
Console.WriteLine($"upload result: {uploadResponse}");


// file download request
using var downloadResponse = await sapi.Send(new FileDownload { Path = @".\example.txt" });
using var reader = new StreamReader(downloadResponse?.Content ?? Stream.Null);
Console.WriteLine($"download result: {await reader.ReadToEndAsync()}");


Console.WriteLine("done");