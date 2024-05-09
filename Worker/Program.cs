using Worker;
using Worker.DummyAbstractions;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IDataService, DataService>();
builder.Services.AddSingleton<IProcessingService, ProcessingService>();
builder.Services.AddHostedService<Worker.BackgroundService>();
builder.Services.AddHostedService<HostedService>();
var host = builder.Build();
host.Run();