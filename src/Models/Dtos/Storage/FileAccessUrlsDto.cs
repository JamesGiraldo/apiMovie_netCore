namespace ApiMovies.Models.Dtos;

// Conjunto de URLs derivadas de un identificador almacenado: pública, vista previa firmada y descarga forzada.
public class FileAccessUrlsDto
{
    public string Url { get; set; } = string.Empty;
    public string UrlPreview { get; set; } = string.Empty;
    public string UrlDownload { get; set; } = string.Empty;
}
