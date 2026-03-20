namespace ApiMovies.Common.Options;

// Opciones de almacenamiento de objetos (S3): bucket, región, URLs públicas, límites de subida y credenciales opcionales.
public class StorageOptions
{
    // Nombre del bucket S3.
    public string BucketName { get; set; } = string.Empty;
    // Región AWS (por ejemplo us-east-1).
    public string Region { get; set; } = string.Empty;
    // Base URL pública; puede incluir el placeholder {BucketName}.
    public string PublicBaseUrl { get; set; } = string.Empty;
    // Tamaño máximo permitido por archivo en megabytes.
    public int MaxFileSizeMb { get; set; } = 5;
    // Vigencia de las URLs firmadas de descarga/visualización.
    public int DownloadUrlExpirationMinutes { get; set; } = 60;
    // Access key explícita; vacío implica cadena de credenciales por defecto del entorno (IAM, perfil, etc.).
    public string AccessKey { get; set; } = string.Empty;
    // Secret key asociada a AccessKey.
    public string SecretKey { get; set; } = string.Empty;
}
