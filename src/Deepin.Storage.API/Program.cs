using Deepin.Storage.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApplicationService();

var app = builder.Build();

app.UseApplicationService();

app.Run();
