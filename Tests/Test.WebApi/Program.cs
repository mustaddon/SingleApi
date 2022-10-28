using MediatR;
using System.Reflection;
using Test.Requests;
using Test.WebApi.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddTransient<IRequestHandler<FileUpload<FileMetadata>, FileUploadResult<FileMetadata>>, FileUploadHandler<FileMetadata>>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = long.MaxValue;
});

var app = builder.Build();

app.MapSingleApi("mediatr", x => x.ServiceProvider.GetRequiredService<IMediator>().Send(x.Data ?? Activator.CreateInstance(x.DataType)!, x.CancellationToken),
    typeof(Ping).Assembly, Assembly.GetExecutingAssembly());

app.MapSingleApi("sapi", async (x) =>
{
    if (x.Data is Stream stream)
    {
        var ms = new MemoryStream();
        await stream.CopyToAsync(ms, x.CancellationToken);
        ms.Position = 0;
        return ms;
    }

    return x.Data;
}, typeof(List<>).Assembly, typeof(int).Assembly);

app.Run();