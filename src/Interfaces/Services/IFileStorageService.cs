using Microsoft.AspNetCore.Http;
using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Services;

public interface IFileStorageService
{
    Task<FileUploadResultDto> UploadImageAsync(
        IFormFile file,
        string folder,
        string entityId,
        CancellationToken cancellationToken = default
    );

    FileAccessUrlsDto GetFileUrls(string? fileUrl);

    Task DeleteByUrlAsync(string? fileUrl, CancellationToken cancellationToken = default);
}
