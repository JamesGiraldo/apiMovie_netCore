using ApiMovies.Interfaces.Repositories;
using ApiMovies.Interfaces.Services;
using ApiMovies.Common.Exceptions;
using ApiMovies.Models.Entities;
using ApiMovies.Models.Dtos;
using AutoMapper;

namespace ApiMovies.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _mRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<MovieService> _logger;
    private readonly IFileStorageService _fileStorageService;

    public MovieService(
        IMovieRepository mRepo,
        IMapper mapper,
        ILogger<MovieService> logger,
        IFileStorageService fileStorageService
    ) {
        _mRepo = mRepo;
        _mapper = mapper;
        _logger = logger;
        _fileStorageService = fileStorageService;
    }

    public IEnumerable<MovieDto> GetMovies(string? search = null) {
        try {
            var movies = string.IsNullOrWhiteSpace(search)
                ? _mRepo.GetMovies()
                : _mRepo.SearchMovies(search.Trim()).ToList();

            if (movies.Count == 0) {
                var detail = string.IsNullOrWhiteSpace(search)
                    ? "No movies were found."
                    : $"No movies were found for search '{search}'.";
                throw new NotFoundException(detail);
            }

            return movies.Select(ToMovieDtoWithUrls).ToList();
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

    public IEnumerable<MovieDto> GetMoviesByCategory(int categoryId, string? search = null) {
        ValidateId(categoryId, "categoryId");
        try {
            var movies = _mRepo.GetMoviesByCategory(categoryId, search);
            if (movies.Count == 0) {
                var detail = string.IsNullOrWhiteSpace(search)
                    ? $"No movies were found for category id {categoryId}."
                    : $"No movies were found in category id {categoryId} for search '{search}'.";
                throw new NotFoundException(detail);
            }

            return movies.Select(ToMovieDtoWithUrls).ToList();
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

    public MovieDto GetMovie(int movieId) {
        ValidateId(movieId, "movieId");
        try {
            var movie = _mRepo.GetMovie(movieId);
            if (movie is null) {
                throw new NotFoundException($"Movie with id {movieId} was not found.");
            }

            return ToMovieDtoWithUrls(movie);
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error getting movie");
            throw new InfrastructureException(
                "An unexpected error occurred while getting the movie.",
                ex
            );
        }
    }

    public MovieDto CreateMovie(MovieCreateDto movieDto) {
        ValidateCreateRequest(movieDto);
        try {
            var movie = MapCreateDtoToMovie(movieDto);
            var created = _mRepo.CreateMovie(movie);
            if (!created) {
                throw new InfrastructureException("Could not persist movie changes.");
            }

            if (movieDto.Image is not null) {
                var uploadResult = _fileStorageService.UploadImageAsync(
                    movieDto.Image,
                    "movies",
                    movie.Name.ToLower().Replace(" ", "-")
                ).GetAwaiter().GetResult();

                movie.FilePath = uploadResult.Url;
                var imageSaved = _mRepo.UpdateMovie(movie);
                if (!imageSaved) {
                    SafeDeleteAsync(uploadResult.Url).GetAwaiter().GetResult();
                    _mRepo.DeleteMovie(movie.Id);
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
        ValidateUpdateRequest(movieId, movieDto);
        try {
            var currentMovie = _mRepo.GetMovie(movieId);
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

            var updated = _mRepo.UpdateMovie(movie);
            if (!updated) {
                await SafeDeleteAsync(uploadResult?.Url);
                throw new InfrastructureException("Could not persist movie changes.");
            }

            if (!string.IsNullOrWhiteSpace(uploadResult?.Url)) {
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

    public MovieDto DeleteMovie(int movieId) {
        ValidateId(movieId, "movieId");
        try {
            var movieToDelete = _mRepo.GetMovie(movieId);
            if (movieToDelete is null) {
                throw new NotFoundException($"Movie with id {movieId} was not found.");
            }

            var deleted = _mRepo.DeleteMovie(movieId);
            if (!deleted) {
                throw new InfrastructureException("Could not persist movie deletion.");
            }

            SafeDeleteAsync(movieToDelete.FilePath).GetAwaiter().GetResult();

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

    /*
     * Methods private to validate the create request
    */

    private void ValidateCreateRequest(MovieCreateDto movieDto) {
        if (movieDto is null) {
            throw new BadRequestException("Movie payload is required.");
        }

        var movieName = movieDto.Name?.Trim();

        if (string.IsNullOrWhiteSpace(movieName)) {
            throw new BadRequestException("Movie name is required.");
        }

        if (_mRepo.ExistsMovieName(movieName)) {
            throw new ConflictException(
                $"The name '{movieDto.Name}' is already in our records. Please use a different movie name."
            );
        }
    }

    private void ValidateUpdateRequest(int movieId, MovieUpdateDto movieDto) {
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

        var currentMovie = _mRepo.GetMovie(movieId);
        if (currentMovie is null) {
            throw new NotFoundException($"Movie with id {movieId} was not found.");
        }

        var isDuplicatedName = _mRepo.ExistsMovieName(movieName)
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

    private MovieDto ToMovieDtoWithUrls(Movie movie) {
        var movieDto = _mapper.Map<MovieDto>(movie);
        var fileUrls = _fileStorageService.GetFileUrls(movie.FilePath);
        movieDto.ImageUrl = fileUrls.UrlPreview;
        // movieDto.FileDownloadUrl = fileUrls.UrlDownload;
        return movieDto;
    }
}