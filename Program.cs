using ApiMovies.Data;
using ApiMovies.Common.Extensions;
using ApiMovies.MoviesMappers;
using Microsoft.AspNetCore.Identity;
using ApiMovies.Models.Entities;


// create a new builder
var builder = WebApplication.CreateBuilder(args);

// builder use Serilog
builder.Host.UseApiSerilog();

// add DbContext to the container with connection string from appsettings.json
builder.Services.AddPostgresDatabase(builder.Configuration);

// add identity to the container
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


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

// add services api versioning
builder.Services.AddApiVersioningConfig();

// build the app
var app = builder.Build();

// use swagger ui
app.UseApiSwaggerUI();

// use the middlewares
app.UseApiMiddlewares();

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

// run the app
app.Run();