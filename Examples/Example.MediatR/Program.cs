using Example;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

var app = builder.Build();

app.MapSingleApi("sapi",
    // invoke the MediatR
    x => x.ServiceProvider.GetRequiredService<IMediator>().Send(x.Data ?? Activator.CreateInstance(x.DataType)!, x.CancellationToken),
    // assemblies for type resolving
    typeof(Ping).Assembly);

app.Run();