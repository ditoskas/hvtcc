using Hvt.Infrastructure.Extensions;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddInfrastructureServices(builder.Configuration);

IHost host = builder.Build();
await DatabaseExtensions.InitialiseDatabaseAsync(builder.Services.BuildServiceProvider());
await host.RunAsync();
