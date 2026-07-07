using LMS.Presentation.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterAllServices(builder.Configuration);

var app = builder.Build();

app.ConfigureWebApplicationMiddlewares();

app.Run();
