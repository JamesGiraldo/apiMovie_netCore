using ApiMovies.Interfaces.Services;
using ApiMovies.Models.Dtos;
using ApiMovies.Models.Entities;
using AutoMapper;

namespace ApiMovies.Services;

public class UserResponseFactory : IUserResponseFactory {
    private readonly IMapper _mapper;
    private readonly IFileStorageService _fileStorageService;
    private readonly ITokenService _tokenService;

    public UserResponseFactory(
        IMapper mapper,
        IFileStorageService fileStorageService,
        ITokenService tokenService
    ) {
        _mapper = mapper;
        _fileStorageService = fileStorageService;
        _tokenService = tokenService;
    }

    public UserResponseDto Create(User user, IList<string> roles) {
        var userInfo = _mapper.Map<UserInfoDto>(user);
        var imageUrls = _fileStorageService.GetFileUrls(user.Image);
        userInfo.Roles = roles.ToList();
        userInfo.Image = imageUrls.Url;
        userInfo.ImageUrl = imageUrls.UrlPreview;

        return new UserResponseDto {
            User = userInfo,
            Expiration = DateTime.UtcNow.AddHours(24),
            Token = _tokenService.GenerateToken(userInfo),
        };
    }
}
