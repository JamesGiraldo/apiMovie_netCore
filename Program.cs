using ApiMovies.Common.Extensions;
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseApiSerilog();
builder.Services.AddApiApplication(builder.Configuration);

var app = builder.Build();

app.UseApiApplication();
app.Run();