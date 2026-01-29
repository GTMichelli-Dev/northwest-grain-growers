using GrainManagement.As400Sync;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<As400SyncOptions>(
    builder.Configuration.GetSection("As400Sync"));

builder.Services.AddHostedService<As400SyncWorker>();

builder.Services.AddSingleton<As400Reader>();
builder.Services.AddSingleton<AccountsUpserter>();
builder.Services.AddSingleton<ProductItemsUpserter>();
builder.Services.AddSingleton<SplitGroupsUpserter>();



var host = builder.Build();
await host.RunAsync();
