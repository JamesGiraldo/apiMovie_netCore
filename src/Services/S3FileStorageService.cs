using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using ApiMovies.Common.Exceptions;
using ApiMovies.Common.Options;
using ApiMovies.Interfaces.Services;
using ApiMovies.Models.Dtos;
using Microsoft.Extensions.Options;

namespace ApiMovies.Services;

// Implementación AWS S3: subida con validación de tipo/tamaño, URLs públicas y firmadas, y borrado por clave derivada de URL.
public class S3FileStorageService : IFileStorageService
{
    private static readonly HashSet<string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/png",
        "image/webp"
    };

    private readonly StorageOptions _options;
    private readonly IAmazonS3 _s3Client;

    public S3FileStorageService(IOptions<StorageOptions> options)
    {
        _options = options.Value;
        ValidateOptions(_options);
        _s3Client = BuildS3Client(_options);
    }

    public async Task<FileUploadResultDto> UploadImageAsync(
        IFormFile file,
        string folder,
        string entityId,
        CancellationToken cancellationToken = default
    ) {
        ValidateFile(file);

        var extension = GetExtension(file.FileName, file.ContentType);
        var objectKey = $"{folder.Trim('/')}/{entityId.Trim()}/{Guid.NewGuid():N}{extension}";

        await using var stream = file.OpenReadStream();
        var request = new PutObjectRequest
        {
            BucketName = _options.BucketName,
            Key = objectKey,
            InputStream = stream,
            ContentType = file.ContentType
        };

        await _s3Client.PutObjectAsync(request, cancellationToken);
        var fileUrls = GetFileUrls(BuildPublicUrl(objectKey));

        return new FileUploadResultDto
        {
            Url = fileUrls.Url,
            UrlDownload = fileUrls.UrlDownload,
            Key = objectKey,
            ContentType = file.ContentType,
            SizeBytes = file.Length
        };
    }

    public FileAccessUrlsDto GetFileUrls(string? fileIdentifier)
    {
        if (string.IsNullOrWhiteSpace(fileIdentifier))
        {
            return new FileAccessUrlsDto();
        }

        var key = ExtractKeyOrKeyPath(fileIdentifier);
        if (string.IsNullOrWhiteSpace(key))
        {
            return new FileAccessUrlsDto
            {
                Url = fileIdentifier,
                UrlPreview = fileIdentifier,
                UrlDownload = fileIdentifier
            };
        }

        var publicUrl = BuildPublicUrl(key);
        var urlPreview = BuildSignedUrl(key, forceDownload: false);
        var urlDownload = BuildSignedUrl(key, forceDownload: true);

        return new FileAccessUrlsDto
        {
            Url = publicUrl,
            UrlPreview = urlPreview,
            UrlDownload = urlDownload
        };
    }

    public async Task DeleteByUrlAsync(string? fileUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
        {
            return;
        }

        var key = ExtractKeyOrKeyPath(fileUrl);
        if (string.IsNullOrWhiteSpace(key))
        {
            return;
        }

        var request = new DeleteObjectRequest
        {
            BucketName = _options.BucketName,
            Key = key
        };

        await _s3Client.DeleteObjectAsync(request, cancellationToken);
    }

    private string BuildPublicUrl(string objectKey)
    {
        var configuredBaseUrl = (_options.PublicBaseUrl ?? string.Empty).Trim();
        var baseUrl = string.IsNullOrWhiteSpace(configuredBaseUrl)
            ? $"https://{_options.BucketName}.s3.{_options.Region}.amazonaws.com"
            : configuredBaseUrl.Replace("{BucketName}", _options.BucketName, StringComparison.OrdinalIgnoreCase);

        return $"{baseUrl.TrimEnd('/')}/{objectKey}";
    }

    private string ExtractKeyOrKeyPath(string fileIdentifier)
    {
        var normalizedInput = fileIdentifier.Trim();

        // If caller already passed a key path (nestjs-style id), use it directly.
        if (!normalizedInput.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !normalizedInput.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            return normalizedInput.TrimStart('/');
        }

        var expectedBaseUrl = BuildPublicUrl(string.Empty).TrimEnd('/');
        if (Uri.TryCreate(normalizedInput, UriKind.Absolute, out var uri))
        {
            if (!normalizedInput.StartsWith(expectedBaseUrl, StringComparison.OrdinalIgnoreCase))
            {
                return string.Empty;
            }

            return uri.AbsolutePath.TrimStart('/');
        }

        return string.Empty;
    }

    private string BuildSignedUrl(string key, bool forceDownload)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _options.BucketName,
            Key = key,
            Verb = HttpVerb.GET,
            Expires = DateTime.UtcNow.AddMinutes(Math.Max(_options.DownloadUrlExpirationMinutes, 1))
        };

        if (forceDownload)
        {
            request.ResponseHeaderOverrides.ContentDisposition =
                $"attachment; filename={Path.GetFileName(key)}";
        }

        return _s3Client.GetPreSignedURL(request);
    }

    private void ValidateFile(IFormFile file)
    {
        if (file is null || file.Length == 0)
        {
            throw new BadRequestException("Image file is required.");
        }

        var maxBytes = Math.Max(_options.MaxFileSizeMb, 1) * 1024L * 1024L;
        if (file.Length > maxBytes)
        {
            throw new BadRequestException($"Image exceeds max allowed size of {_options.MaxFileSizeMb} MB.");
        }

        if (!AllowedContentTypes.Contains(file.ContentType))
        {
            throw new BadRequestException("Invalid image format. Allowed: jpeg, png, webp.");
        }
    }

    private static string GetExtension(string fileName, string contentType)
    {
        var extension = Path.GetExtension(fileName)?.ToLowerInvariant();
        if (!string.IsNullOrWhiteSpace(extension))
        {
            return extension;
        }

        return contentType.ToLowerInvariant() switch
        {
            "image/jpeg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            _ => ".bin"
        };
    }

    private static void ValidateOptions(StorageOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.BucketName))
        {
            throw new InfrastructureException("Storage:BucketName is not configured.");
        }

        if (string.IsNullOrWhiteSpace(options.Region))
        {
            throw new InfrastructureException("Storage:Region is not configured.");
        }
    }

    private static IAmazonS3 BuildS3Client(StorageOptions options)
    {
        var region = RegionEndpoint.GetBySystemName(options.Region);

        if (!string.IsNullOrWhiteSpace(options.AccessKey) && !string.IsNullOrWhiteSpace(options.SecretKey))
        {
            var credentials = new BasicAWSCredentials(options.AccessKey, options.SecretKey);
            return new AmazonS3Client(credentials, region);
        }

        return new AmazonS3Client(region);
    }
}
