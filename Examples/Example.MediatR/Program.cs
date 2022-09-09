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