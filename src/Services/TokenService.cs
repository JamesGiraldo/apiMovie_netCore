using ApiMovies.Common.Exceptions;
using ApiMovies.Interfaces.Services;
using ApiMovies.Models.Dtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiMovies.Services;

public class TokenService : ITokenService {
    private readonly IConfigurationSection _secretKey;

    public TokenService(IConfiguration config) {
        _secretKey = config.GetSection("ApiSettings:SecretKey");
    }

    public string GenerateToken(UserInfoDto userInfoDto) {
        var tokenHandler = new JwtSecurityTokenHandler();
        var signingKey = GetSigningKey();
        var claims = BuildClaims(userInfoDto);
        var tokenDescriptor = BuildTokenDescriptor(claims, signingKey);

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private SymmetricSecurityKey GetSigningKey() {
        var secretKey = _secretKey.Value;
        if (string.IsNullOrWhiteSpace(secretKey)) {
            throw new InfrastructureException("ApiSettings:SecretKey is not configured.");
        }

        var key = Encoding.UTF8.GetBytes(secretKey);
        return new SymmetricSecurityKey(key);
    }

    private static Claim[] BuildClaims(UserInfoDto userInfoDto) {
        return new[] {
            new Claim(ClaimTypes.Name, userInfoDto.UserName),
            new Claim(ClaimTypes.Email, userInfoDto.Email),
            new Claim(ClaimTypes.Role, string.Join(",", userInfoDto.Roles))
        };
    }

    private static SecurityTokenDescriptor BuildTokenDescriptor(
        IEnumerable<Claim> claims,
        SymmetricSecurityKey signingKey
    ) {
        return new SecurityTokenDescriptor {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(24),
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256Signature)
        };
    }
}
