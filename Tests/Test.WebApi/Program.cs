using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.MapSingleApi("mediatr", x => x.ServiceProvider.GetRequiredService<IMediator>().Send(x.Data, x.CancellationToken),
    typeof(Test.Ping).Assembly);

app.MapSingleApi("sapi", x => Task.FromResult(x.Data),
    typeof(List<>).Assembly, typeof(int).Assembly);

app.Run();