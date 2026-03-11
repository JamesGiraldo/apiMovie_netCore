using ApiMovies.Interfaces.Repositories;
using ApiMovies.Interfaces.Services;
using ApiMovies.Models;
using ApiMovies.Models.Dtos;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ApiMovies.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _mRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<MovieService> _logger;

    public MovieService(
        IMovieRepository mRepo,
        IMapper mapper,
        ILogger<MovieService> logger
    ) {
        _mRepo = mRepo;
        _mapper = mapper;
        _logger = logger;
    }

    public ServiceResult<IEnumerable<MovieDto>> GetMovies(string? search = null) {
        try {
            var movies = string.IsNullOrWhiteSpace(search)
                ? _mRepo.GetMovies()
                : _mRepo.SearchMovies(search.Trim()).ToList();

            if (movies.Count == 0) {
                return FailureList(
                    "NotFound",
                    "Movies not found.",
                    string.IsNullOrWhiteSpace(search)
                        ? "No movies were found."
                        : $"No movies were found for search '{search}'."
                );
            }

            var moviesDto = _mapper.Map<IEnumerable<MovieDto>>(movies);
            return ServiceResult<IEnumerable<MovieDto>>.Success(moviesDto);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error getting movies");
            return FailureList(
                "Unexpected",
                "Could not retrieve movies.",
                "An unexpected error occurred while retrieving movies."
            );
        }
    }

    public ServiceResult<IEnumerable<MovieDto>> GetMoviesByCategory(int categoryId, string? search = null) {
        try {
            if (categoryId <= 0) {
                return FailureList(
                    "InvalidId",
                    "Invalid category id.",
                    "categoryId must be greater than 0."
                );
            }

            var movies = _mRepo.GetMoviesByCategory(categoryId, search);
            if (movies.Count == 0) {
                return FailureList(
                    "NotFound",
                    "Movies not found.",
                    string.IsNullOrWhiteSpace(search)
                        ? $"No movies were found for category id {categoryId}."
                        : $"No movies were found in category id {categoryId} for search '{search}'."
                );
            }

            var moviesDto = _mapper.Map<IEnumerable<MovieDto>>(movies);
            return ServiceResult<IEnumerable<MovieDto>>.Success(moviesDto);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error getting movies by category");
            return FailureList(
                "Unexpected",
                "Could not retrieve movies by category.",
                "An unexpected error occurred while retrieving movies by category."
            );
        }
    }

    public ServiceResult<MovieDto> GetMovie(int movieId) {
        try {
            if (movieId <= 0) {
                return Failure(
                    "InvalidId",
                    "Invalid movie id.",
                    "movieId must be greater than 0."
                );
            }

            var movie = _mRepo.GetMovie(movieId);
            if (movie is null) {
                return Failure(
                    "NotFound",
                    "Movie not found.",
                    $"Movie with id {movieId} was not found."
                );
            }

            return ServiceResult<MovieDto>.Success(_mapper.Map<MovieDto>(movie));
        } catch (Exception ex) {
            _logger.LogError(ex, "Error getting movie");
            return Failure(
                "Unexpected",
                "Could not get movie",
                "An unexpected error occurred while getting the movie."
            );
        }
    }

    public ServiceResult<MovieDto> CreateMovie(MovieCreateDto movieDto) {
        var validation = ValidateCreateRequest(movieDto);
        if (!validation.Succeeded) return validation;
        try
        {
            var movie = MapCreateDtoToMovie(movieDto);
            var created = _mRepo.CreateMovie(movie);
            if (!created) {
                return Failure(
                    "Persistence",
                    "Could not create movie",
                    "Could not persist movie changes"
                );
            }

            return ServiceResult<MovieDto>.Success(_mapper.Map<MovieDto>(movie));
        } catch (Exception ex) {
            _logger.LogError(ex, "Error creating movie");
            return Failure(
                "Unexpected",
                "Could not create movie",
                "An unexpected error occurred while creating the movie."
            );
        }
    }

    public ServiceResult<MovieDto> UpdateMovie(int movieId, MovieDto movieDto) {
        var validation = ValidateUpdateRequest(movieId, movieDto);
        if (!validation.Succeeded) return validation;
        try
        {
            var movie = MapUpdateDtoToMovie(movieId, movieDto);
            var updated = _mRepo.UpdateMovie(movie);
            if (!updated) {
                return Failure(
                    "Persistence",
                    "Could not update movie",
                    "Could not persist movie changes"
                );
            }

            return ServiceResult<MovieDto>.Success(_mapper.Map<MovieDto>(movie));
        } catch (Exception ex) {
            _logger.LogError(ex, "Error updating movie");
            return Failure(
                "Unexpected",
                "Could not update movie",
                "An unexpected error occurred while updating the movie."
            );
        }
    }

    public ServiceResult<MovieDto> ReplaceMovie(int movieId, MovieDto movieDto) {
        var validation = ValidateUpdateRequest(movieId, movieDto);
        if (!validation.Succeeded) return validation;
        try
        {
            var movie = MapUpdateDtoToMovie(movieId, movieDto);

            var replaced = _mRepo.UpdateMovie(movie);
            if (!replaced) {
                return Failure(
                    "Persistence",
                    "Could not replace movie changes",
                    "Could not persist movie changes"
                );
            }

            return ServiceResult<MovieDto>.Success(_mapper.Map<MovieDto>(movie));
        } catch (Exception ex) {
            _logger.LogError(ex, "Error replacing movie");
            return Failure(
                "Unexpected",
                "Could not replace movie changes",
                "An unexpected error occurred while replacing the movie changes."
            );
        }
    }

    public ServiceResult<MovieDto> DeleteMovie(int movieId) {
        var validation = ValidateDeleteRequest(movieId);
        if (!validation.Succeeded) return validation;
        try {
            var movieToDelete = _mRepo.GetMovie(movieId);
            if (movieToDelete is null) {
                return Failure(
                    "NotFound",
                    "Movie not found.",
                    $"Movie with id {movieId} was not found."
                );
            }

            var deleted = _mRepo.DeleteMovie(movieId);
            if (!deleted) {
                return Failure(
                    "Persistence",
                    "Could not delete movie",
                    "Could not persist movie deletion"
                );
            }

            return ServiceResult<MovieDto>.Success(_mapper.Map<MovieDto>(movieToDelete));
        } catch (Exception ex) {
            _logger.LogError(ex, "Error deleting movie");
            return Failure(
                "Unexpected",
                "Could not delete movie",
                "An unexpected error occurred while deleting the movie."
            );
        }
    }

    /*
     * Methods private to validate the create request
    */

    private ServiceResult<MovieDto> ValidateCreateRequest(MovieCreateDto movieDto) {
        if (movieDto is null) {
            return Failure(
                "InvalidPayload",
                "Invalid request payload.",
                "Movie payload is required."
            );
        }

        var movieName = movieDto.Name?.Trim();

        if (string.IsNullOrWhiteSpace(movieName)) {
            return Failure(
                "InvalidName",
                "Invalid movie name.",
                "Movie name is required."
            );
        }

        if (_mRepo.ExistsMovieName(movieName)) {
            return Failure(
                "DuplicateName",
                "Movie name already exists.",
                $"The name '{movieDto.Name}' is already in our records. Please use a different movie name."
            );
        }
        return ServiceResult<MovieDto>.Success(default!);
    }

    private ServiceResult<MovieDto> ValidateUpdateRequest(int movieId, MovieDto movieDto) {
        if (movieId <= 0) {
            return Failure(
                "InvalidId",
                "Invalid movie id.",
                "movieId must be greater than 0."
            );
        }

        if (movieDto is null) {
            return Failure(
                "InvalidPayload",
                "Invalid request payload.",
                "Movie payload is required."
            );
        }

        if (movieDto.Id > 0 && movieDto.Id != movieId) {
            return Failure(
                "RouteBodyIdMismatch",
                "Route id and body id do not match.",
                $"Route id '{movieId}' must match body id '{movieDto.Id}'."
            );
        }

        var movieName = movieDto.Name?.Trim();

        if (string.IsNullOrWhiteSpace(movieName)) {
            return Failure(
                "InvalidName",
                "Invalid movie name.",
                "Movie name is required."
            );
        }

        var currentMovie = _mRepo.GetMovie(movieId);
        if (currentMovie is null) {
            return Failure(
                "NotFound",
                "Movie not found.",
                $"Movie with id {movieId} was not found."
            );
        }

        var isDuplicatedName = _mRepo.ExistsMovieName(movieName)
            && !string.Equals(currentMovie.Name, movieName, StringComparison.OrdinalIgnoreCase);

        if (isDuplicatedName) {
            return Failure(
                "DuplicateName",
                "Movie name already exists.",
                $"The name '{movieDto.Name}' is already in our records. Please use a different movie name."
            );
        }

        return ServiceResult<MovieDto>.Success(default!);
    }

    private ServiceResult<MovieDto> ValidateDeleteRequest(int movieId) {
        if (movieId <= 0) {
            return Failure(
                "InvalidId",
                "Invalid movie id.",
                "movieId must be greater than 0."
            );
        }

        var currentMovie = _mRepo.GetMovie(movieId);
        if (currentMovie is null) {
            return Failure(
                "NotFound",
                "Movie not found.",
                $"Movie with id {movieId} was not found."
            );
        }

        return ServiceResult<MovieDto>.Success(default!);
    }

    private Movie MapCreateDtoToMovie(MovieCreateDto dto) {
        var movie = _mapper.Map<Movie>(dto);
        movie.Name = dto.Name.Trim();
        return movie;
    }

    private Movie MapUpdateDtoToMovie(int movieId, MovieDto dto) {
        var movie = _mapper.Map<Movie>(dto);
        movie.Id = movieId;
        return movie;
    }

    private static ServiceResult<MovieDto> Failure(
        string code,
        string title,
        string detail
    ) {
        return ServiceResult<MovieDto>.Failure(code, title, detail);
    }

    private static ServiceResult<IEnumerable<MovieDto>> FailureList(
        string code,
        string title,
        string detail
    ) {
        return ServiceResult<IEnumerable<MovieDto>>.Failure(code, title, detail);
    }
}