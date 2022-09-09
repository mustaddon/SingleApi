using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

var app = builder.Build();

app.MapSingleApi("sapi", x => x.ServiceProvider.GetRequiredService<IMediator>().Send(x.Data, x.CancellationToken),
    typeof(Test.Ping).Assembly, typeof(List<>).Assembly, typeof(int).Assembly);

app.Run();