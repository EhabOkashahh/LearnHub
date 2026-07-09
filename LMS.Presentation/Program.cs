using LMS.Presentation.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterAllServices();

var app = builder.Build();

app.ConfigureWebApplicationMiddlewares();

app.Run();
