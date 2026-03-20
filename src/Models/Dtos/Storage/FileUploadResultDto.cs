namespace ApiMovies.Models.Dtos;

// Resultado inmediato tras subir un objeto al bucket: URLs, clave S3 y metadatos del archivo.
public class FileUploadResultDto
{
    // URL pública o canónica del objeto.
    public string Url { get; set; } = string.Empty;
    // URL firmada orientada a descarga.
    public string UrlDownload { get; set; } = string.Empty;
    // Clave completa dentro del bucket (carpeta/entidad/guid.ext).
    public string Key { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public long SizeBytes { get; set; }
}
