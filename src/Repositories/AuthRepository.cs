using ApiMovies.Interfaces.Repositories;
using ApiMovies.Models.Dtos;
using ApiMovies.Models.Entities;
using ApiMovies.Common.Exceptions;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
namespace ApiMovies.Repositories;

public class AuthRepository : IAuthRepository {

    private readonly IUserRepository _userRepository;
    private readonly IConfigurationSection _secretKey;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMapper _mapper;

    public AuthRepository(
        IUserRepository userRepository,
        IConfiguration config,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper
    ) {
        _userRepository = userRepository;
        _secretKey = config.GetSection("ApiSettings:SecretKey");
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
    }

    public async Task<UserResponseDto> LoginUser(UserLoginDto userLoginDto) {
        var user = await _userRepository.GetByUserNameOrEmail(
            userLoginDto.UserName,
            userLoginDto.Email
        );

        if (user is null) throw new NotFoundException("User not found or invalid user name or email.");

        var isValid = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);
        if (!isValid) throw new NotFoundException("User not found or invalid user name or email or password.");

        var roles = await _userManager.GetRolesAsync(user);
        var userInfo = _mapper.Map<UserInfoDto>(user);

        userInfo.Roles = roles.ToList();
        var token = GenerateToken(userInfo);

        return new UserResponseDto {
            User = userInfo,
            Expiration = DateTime.UtcNow.AddHours(24),
            Token = token,
        };
    }

    public async Task<UserResponseDto> RegisterUser(UserCreateDto userCreateDto) {

        User user = new User() {
            Email = userCreateDto.Email,
            LastName = userCreateDto.LastName,
            Name = userCreateDto.Name,
            UserName = userCreateDto.UserName,
            PhoneNumber = userCreateDto.PhoneNumber,
            NormalizedEmail = userCreateDto.Email.ToUpper(),
            NormalizedUserName = userCreateDto.UserName.ToUpper(),
        };

        var result = await _userManager.CreateAsync(user, userCreateDto.Password);
        if (!result.Succeeded) {
            var errors = string.Join(" | ", result.Errors.Select(e => e.Description));
            throw new BadRequestException(errors);
        }

        if (!_roleManager.RoleExistsAsync("Admin").GetAwaiter().GetResult()) {
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
            await _roleManager.CreateAsync(new IdentityRole("Registered"));
        }

        await _userManager.AddToRoleAsync(user, "Admin");

        var userInfo = _mapper.Map<UserInfoDto>(user);
        var roles = await _userManager.GetRolesAsync(user);
        userInfo.Roles = roles.ToList();
        var token = GenerateToken(userInfo);

        return new UserResponseDto {
            User = userInfo,
            Expiration = DateTime.UtcNow.AddHours(24),
            Token = token,
        };
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