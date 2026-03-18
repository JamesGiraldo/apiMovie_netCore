namespace ApiMovies.Models.Dtos;

public class FileUploadResultDto
{
    public string Url { get; set; } = string.Empty;
    public string UrlDownload { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
}
