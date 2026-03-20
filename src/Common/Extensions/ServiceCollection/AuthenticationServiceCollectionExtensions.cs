using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ApiMovies.Common.Extensions;

// Autenticación stateless: valida el JWT firmado con la clave simétrica de configuración.
public static class AuthenticationServiceCollectionExtensions
{
    // Usa ApiSettings:SecretKey para firmar/validar tokens (issuer/audience desactivados en este ejemplo).
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var secretKey = configuration.GetValue<string>("ApiSettings:SecretKey")
            ?? throw new InvalidOperationException("ApiSettings:SecretKey is missing in configuration.");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
            };
        });

        return services;
    }
}
