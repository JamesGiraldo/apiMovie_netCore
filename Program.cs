using ApiMovies.Data;
using ApiMovies.Common.Extensions;
using ApiMovies.Common.Middlewares;
using ApiMovies.MoviesMappers;

// create a new builder
var builder = WebApplication.CreateBuilder(args);

// builder use Serilog
builder.Host.UseApiSerilog();

// add DbContext to the container with connection string from appsettings.json
builder.Services.AddPostgresDatabase(builder.Configuration);

// add repositories
builder.Services.AddRepositories();

// add services
builder.Services.AddServices();

// add AutoMapper
builder.Services.AddAutoMapper(_ => { }, typeof(MoviesMapper).Assembly);

// add authentication
builder.Services.AddJwtAuthentication(builder.Configuration);

// add controllers
builder.Services.AddApiControllers();

// add services addEndpointsApiExplorer
builder.Services.AddEndpointsApiExplorer();

// add services addSwaggerGen
builder.Services.AddApiSwagger();

// add services addCors
builder.Services.AddApiCors();

// build the app
var app = builder.Build();

// if the environment is development, use swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// use the middlewares
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<RequestTimingMiddleware>();

// use the authentication and authorization
app.UseHttpsRedirection();
// use the cors
app.UseCors("AllowAll");
// use the authentication
app.UseAuthentication();
// use the authorization
app.UseAuthorization();

// map the controllers
app.MapControllers();

// map the endpoints
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// map the weather forecast
app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

// run the app
app.Run();

// record the weather forecast
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
