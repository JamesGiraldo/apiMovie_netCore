using ApiMovies.Interfaces.Repositories;
using ApiMovies.Models.Dtos;
using ApiMovies.Models.Entities;
using Mapster;
using BCrypt.Net;
using ApiMovies.Common.Exceptions;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace ApiMovies.Repositories;

public class AuthRepository : IAuthRepository {

    private readonly IUserRepository _userRepository;
    private readonly IConfigurationSection _secretKey;

    public AuthRepository(IUserRepository userRepository, IConfiguration config) {
        _userRepository = userRepository;
        _secretKey = config.GetSection("ApiSettings:SecretKey");
    }

    public async Task<UserResponseDto> LoginUser(UserLoginDto userLoginDto) {
        var user = await _userRepository.GetByUserNameOrEmail(
            userLoginDto.UserName,
            userLoginDto.Email
        );

        if (user is null) throw new NotFoundException("User not found or invalid user name or email.");

        var userInfoDto = user.Adapt<UserInfoDto>();

        if (!VerifyPassword(userLoginDto.Password, user.Password)) {
            throw new BadRequestException("Invalid password.");
        }

        var token = GenerateToken(userInfoDto);
        return new UserResponseDto { Token = token, User = userInfoDto, Expiration = DateTime.UtcNow };
    }

    public async Task<UserResponseDto> RegisterUser(UserCreateDto userCreateDto) {

        var passwordEncrypted = GetPasswordEncrypted(userCreateDto.Password);

        var user = new User {
            Email = userCreateDto.Email,
            IsActive = true,
            Name = userCreateDto.Name,
            Password = passwordEncrypted,
            Role = userCreateDto.Role,
            UserName = userCreateDto.UserName,
        };

        if (!await _userRepository.CreateUser(user)) return null!;

        var userInfoDto = user.Adapt<UserInfoDto>();

        var token = GenerateToken(userInfoDto);
        return new UserResponseDto { Token = token, User = userInfoDto, Expiration = DateTime.UtcNow };
    }

    private string GetPasswordEncrypted(string password) {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hashedPassword) {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    private string GenerateToken(UserInfoDto userInfoDto) {
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
        return new[] { new Claim(ClaimTypes.Name, userInfoDto.UserName) };
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