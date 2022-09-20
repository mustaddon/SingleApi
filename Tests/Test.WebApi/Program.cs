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

app.MapSingleApi("mediatr", x => x.ServiceProvider.GetRequiredService<IMediator>().Send(x.Data ?? Activator.CreateInstance(x.DataType) ?? new object(), x.CancellationToken),
    typeof(Ping).Assembly, Assembly.GetExecutingAssembly());

app.MapSingleApi("sapi", x => Task.FromResult(x.Data),
    typeof(List<>).Assembly, typeof(int).Assembly);

app.Run();