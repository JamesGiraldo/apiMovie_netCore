namespace ApiMovies.Common.Options;

public class StorageOptions
{
    public string BucketName { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string PublicBaseUrl { get; set; } = string.Empty;
    public int MaxFileSizeMb { get; set; } = 5;
    public int DownloadUrlExpirationMinutes { get; set; } = 60;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
}
