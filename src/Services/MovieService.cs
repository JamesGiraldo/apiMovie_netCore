using ApiMovies.Interfaces.Repositories;
using ApiMovies.Interfaces.Services;
using ApiMovies.Common.Exceptions;
using ApiMovies.Common.Pagination;
using ApiMovies.Models.Entities;
using ApiMovies.Models.Dtos;
using AutoMapper;

namespace ApiMovies.Services;

// Reglas de películas: búsqueda, CRUD, unicidad de nombre y ciclo de vida de imagen en almacenamiento S3.
public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<MovieService> _logger;
    private readonly IFileStorageService _fileStorageService;

    public MovieService(
        IMovieRepository movieRepository,
        IMapper mapper,
        ILogger<MovieService> logger,
        IFileStorageService fileStorageService
    ) {
        _movieRepository = movieRepository;
        _mapper = mapper;
        _logger = logger;
        _fileStorageService = fileStorageService;
    }

    public async Task<PagedResult<MovieDto>> GetMovies(string? search = null, PaginationQuery? paginationQuery = null) {
        try {
            var movies = string.IsNullOrWhiteSpace(search)
                ? await _movieRepository.GetMovies()
                : await _movieRepository.SearchMovies(search.Trim());

            if (movies.Count == 0) {
                var detail = string.IsNullOrWhiteSpace(search)
                    ? "No movies were found."
                    : $"No movies were found for search '{search}'.";
                throw new NotFoundException(detail);
            }

            var pagedMovies = movies.ToPagedResult(paginationQuery);
            var moviesResponse = pagedMovies.Items.Select(ToMovieDtoWithUrls).ToList();

            return new PagedResult<MovieDto>(
                moviesResponse,
                pagedMovies.TotalCount,
                pagedMovies.PageNumber,
                pagedMovies.PageSize
            );
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error getting movies");
            throw new InfrastructureException(
                "An unexpected error occurred while retrieving movies.",
                ex
            );
        }
    }

    public async Task<PagedResult<MovieDto>> GetMoviesByCategory(
        int categoryId,
        string? search = null,
        PaginationQuery? paginationQuery = null
    ) {
        ValidateId(categoryId, "categoryId");
        try {
            var movies = await _movieRepository.GetMoviesByCategory(categoryId, search);
            if (movies.Count == 0) {
                var detail = string.IsNullOrWhiteSpace(search)
                    ? $"No movies were found for category id {categoryId}."
                    : $"No movies were found in category id {categoryId} for search '{search}'.";
                throw new NotFoundException(detail);
            }

            var pagedMovies = movies.ToPagedResult(paginationQuery);
            var moviesResponse = pagedMovies.Items.Select(ToMovieDtoWithUrls).ToList();

            return new PagedResult<MovieDto>(
                moviesResponse,
                pagedMovies.TotalCount,
                pagedMovies.PageNumber,
                pagedMovies.PageSize
            );
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error getting movies by category");
            throw new InfrastructureException(
                "An unexpected error occurred while retrieving movies by category.",
                ex
            );
        }
    }

    public async Task<MovieDto> GetMovie(int movieId) {
        ValidateId(movieId, "movieId");
        try {
            var movie = await _movieRepository.GetMovie(movieId);
            if (movie is null) {
                throw new NotFoundException($"Movie with id {movieId} was not found.");
            }

            return ToMovieDtoWithUrls(movie);
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error getting movie");
            throw new InfrastructureException(
                "An unexpected error occurred while retrieving movie.",
                ex
            );
        }
    }

    public async Task<MovieDto> CreateMovie(MovieCreateDto movieDto) {
        await ValidateCreateRequestAsync(movieDto);
        try {
            var movie = MapCreateDtoToMovie(movieDto);
            var created = await _movieRepository.CreateMovie(movie);
            if (!created) {
                throw new InfrastructureException("Could not persist movie changes.");
            }

            if (movieDto.Image is not null) {
                var uploadResult = await _fileStorageService.UploadImageAsync(
                    movieDto.Image,
                    "movies",
                    movie.Name.ToLower().Replace(" ", "-")
                );

                movie.FilePath = uploadResult.Url;
                var imageSaved = await _movieRepository.UpdateMovie(movie);
                if (!imageSaved) {
                    // Rollback: quitar archivo subido y fila creada si no se pudo guardar la ruta.
                    await SafeDeleteAsync(uploadResult.Url);
                    await _movieRepository.DeleteMovie(movie.Id);
                    throw new InfrastructureException("Could not persist movie image changes.");
                }
            }

            return ToMovieDtoWithUrls(movie);
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error creating movie");
            throw new InfrastructureException(
                "An unexpected error occurred while creating the movie.",
                ex
            );
        }
    }

    public async Task<MovieDto> UpdateMovie(int movieId, MovieUpdateDto movieDto) {
        await ValidateUpdateRequestAsync(movieId, movieDto);
        try {
            var currentMovie = await _movieRepository.GetMovie(movieId);
            if (currentMovie is null) {
                throw new NotFoundException($"Movie with id {movieId} was not found.");
            }

            var previousImageUrl = currentMovie.FilePath;
            FileUploadResultDto? uploadResult = null;
            if (movieDto.Image is not null) {
                uploadResult = await _fileStorageService.UploadImageAsync(
                    movieDto.Image,
                    "movies",
                    movieDto.Name.ToLower().Replace(" ", "-")
                );
            }

            var movie = MapUpdateDtoToMovie(movieId, movieDto);
            if (string.IsNullOrWhiteSpace(movie.FilePath)) {
                movie.FilePath = currentMovie.FilePath;
            }
            if (!string.IsNullOrWhiteSpace(uploadResult?.Url)) {
                movie.FilePath = uploadResult.Url;
            }

            var updated = await _movieRepository.UpdateMovie(movie);
            if (!updated) {
                await SafeDeleteAsync(uploadResult?.Url);
                throw new InfrastructureException("Could not persist movie changes.");
            }

            if (!string.IsNullOrWhiteSpace(uploadResult?.Url)) {
                // Sustitución de imagen: borrar la anterior solo tras persistir la nueva URL.
                await SafeDeleteAsync(previousImageUrl);
            }

            return ToMovieDtoWithUrls(movie);
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error updating movie");
            throw new InfrastructureException(
                "An unexpected error occurred while updating the movie.",
                ex
            );
        }
    }

    public async Task<MovieDto> ReplaceMovie(int movieId, MovieUpdateDto movieDto) {
        return await UpdateMovie(movieId, movieDto);
    }

    public async Task<MovieDto> DeleteMovie(int movieId) {
        ValidateId(movieId, "movieId");
        try {
            var movieToDelete = await _movieRepository.GetMovie(movieId);
            if (movieToDelete is null) {
                throw new NotFoundException($"Movie with id {movieId} was not found.");
            }

            var deleted = await _movieRepository.DeleteMovie(movieId);
            if (!deleted) {
                throw new InfrastructureException("Could not persist movie deletion.");
            }

            await SafeDeleteAsync(movieToDelete.FilePath);

            return ToMovieDtoWithUrls(movieToDelete);
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error deleting movie");
            throw new InfrastructureException(
                "An unexpected error occurred while deleting the movie.",
                ex
            );
        }
    }

    // Valida payload de alta y nombre único antes de tocar la base de datos.
    private async Task ValidateCreateRequestAsync(MovieCreateDto movieDto) {
        if (movieDto is null) {
            throw new BadRequestException("Movie payload is required.");
        }

        var movieName = movieDto.Name?.Trim();

        if (string.IsNullOrWhiteSpace(movieName)) {
            throw new BadRequestException("Movie name is required.");
        }

        if (await _movieRepository.ExistsMovieName(movieName)) {
            throw new ConflictException(
                $"The name '{movieDto.Name}' is already in our records. Please use a different movie name."
            );
        }
    }

    private async Task ValidateUpdateRequestAsync(int movieId, MovieUpdateDto movieDto) {
        ValidateId(movieId, "movieId");

        if (movieDto is null) {
            throw new BadRequestException("Movie payload is required.");
        }

        if (movieDto.Id > 0 && movieDto.Id != movieId) {
            throw new BadRequestException(
                $"Route id '{movieId}' must match body id '{movieDto.Id}'."
            );
        }

        var movieName = movieDto.Name?.Trim();

        if (string.IsNullOrWhiteSpace(movieName)) {
            throw new BadRequestException("Movie name is required.");
        }

        var currentMovie = await _movieRepository.GetMovie(movieId);
        if (currentMovie is null) {
            throw new NotFoundException($"Movie with id {movieId} was not found.");
        }

        var isDuplicatedName = await _movieRepository.ExistsMovieName(movieName)
            && !string.Equals(currentMovie.Name, movieName, StringComparison.OrdinalIgnoreCase);

        if (isDuplicatedName) {
            throw new ConflictException(
                $"The name '{movieDto.Name}' is already in our records. Please use a different movie name."
            );
        }
    }

    private Movie MapCreateDtoToMovie(MovieCreateDto dto) {
        var movie = _mapper.Map<Movie>(dto);
        movie.Name = dto.Name.Trim();
        return movie;
    }

    private Movie MapUpdateDtoToMovie(int movieId, MovieUpdateDto dto) {
        var movie = _mapper.Map<Movie>(dto);
        movie.Id = movieId;
        movie.Name = dto.Name.Trim();
        return movie;
    }

    private static void ValidateId(int id, string paramName) {
        if (id <= 0) {
            throw new BadRequestException($"{paramName} must be greater than 0.");
        }
    }

    private async Task SafeDeleteAsync(string? fileUrl) {
        if (string.IsNullOrWhiteSpace(fileUrl)) {
            return;
        }

        try {
            await _fileStorageService.DeleteByUrlAsync(fileUrl);
        } catch (Exception ex) {
            _logger.LogWarning(ex, "Could not delete old movie image {FileUrl}", fileUrl);
        }
    }

    // Enriquece el DTO con URL de vista previa firmada (descarga opcional comentada en código).
    private MovieDto ToMovieDtoWithUrls(Movie movie) {
        var movieDto = _mapper.Map<MovieDto>(movie);
        var fileUrls = _fileStorageService.GetFileUrls(movie.FilePath);
        movieDto.ImageUrl = fileUrls.UrlPreview;
        // movieDto.FileDownloadUrl = fileUrls.UrlDownload;
        return movieDto;
    }
}