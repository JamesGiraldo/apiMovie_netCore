using Microsoft.AspNetCore.Http;
using ApiMovies.Models.Dtos;

namespace ApiMovies.Interfaces.Services;

// Abstracción de almacenamiento de archivos (p. ej. S3): subida de imágenes, URLs firmadas y borrado compensatorio.
public interface IFileStorageService
{
    // Sube un archivo de imagen validado (tipo y tamaño) bajo una clave jerárquica folder/entityId/guid.ext.
    // Parámetro file: Archivo multipart del request.
    // Parámetro folder: Segmento inicial de la clave (p. ej. users, movies).
    // Parámetro entityId: Identificador estable para agrupar objetos del mismo recurso.
    // Parámetro cancellationToken: Cancelación.
    // Retorna: URL pública, URL de descarga firmada y metadatos.
    Task<FileUploadResultDto> UploadImageAsync(
        IFormFile file,
        string folder,
        string entityId,
        CancellationToken cancellationToken = default
    );

    // Resuelve URLs de vista previa y descarga a partir de una URL pública, clave S3 o identificador almacenado en BD.
    FileAccessUrlsDto GetFileUrls(string? fileUrl);

    // Elimina el objeto en el bucket si fileUrl se puede resolver a una clave del bucket configurado.
    Task DeleteByUrlAsync(string? fileUrl, CancellationToken cancellationToken = default);
}
