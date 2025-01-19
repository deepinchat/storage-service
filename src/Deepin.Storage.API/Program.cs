using Deepin.Storage.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:5000");

builder.AddApplicationService();

var app = builder.Build();

app.UseApplicationService();

app.Run();
