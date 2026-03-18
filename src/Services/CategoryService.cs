using ApiMovies.Interfaces.Repositories;
using ApiMovies.Interfaces.Services;
using ApiMovies.Common.Exceptions;
using ApiMovies.Models.Entities;
using ApiMovies.Models.Dtos;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ApiMovies.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(
        ICategoryRepository categoryRepository,
        IMapper mapper,
        ILogger<CategoryService> logger
    ) {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<CategoryDto>> GetCategories(string? search = null) {
        try {
            var categories = await _categoryRepository.GetCategories(search);
            var hasSearch = !string.IsNullOrWhiteSpace(search);

            if (!hasSearch && categories.Count == 0) {
                throw new NotFoundException("No categories were found.");
            }

            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);

            return categoriesDto;
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error getting categories");
            throw new InfrastructureException(
                "An unexpected error occurred while retrieving categories.",
                ex
            );
        }
    }

    public async Task<CategoryDto> GetCategory(int categoryId) {
        ValidateId(categoryId);
        try {
            var category = await _categoryRepository.GetCategory(categoryId);
            if (category is null) {
                throw new NotFoundException($"Category with id {categoryId} was not found.");
            }

            return _mapper.Map<CategoryDto>(category);
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error getting category");
            throw new InfrastructureException(
                "An unexpected error occurred while retrieving category.",
                ex
            );
        }
    }

    public async Task<CategoryDto> CreateCategory(CategoryCreateDto categoryDto) {
        await ValidateCreateRequestAsync(categoryDto);
        try {
            var category = MapCreateDtoToCategory(categoryDto);
            var created = await _categoryRepository.CreateCategory(category);
            if (!created) {
                throw new InfrastructureException("Could not persist category changes.");
            }

            return _mapper.Map<CategoryDto>(category);
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error creating category");
            throw new InfrastructureException(
                "An unexpected error occurred while creating the category.",
                ex
            );
        }
    }

    public async Task<CategoryDto> UpdateCategory(int categoryId, CategoryDto categoryDto) {
        await ValidateUpdateRequestAsync(categoryId, categoryDto);
        try {
            var category = MapUpdateDtoToCategory(categoryId, categoryDto);
            var updated = await _categoryRepository.UpdateCategory(category);
            if (!updated) {
                throw new InfrastructureException("Could not persist category changes.");
            }

            return _mapper.Map<CategoryDto>(category);
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error updating category");
            throw new InfrastructureException(
                "An unexpected error occurred while updating the category.",
                ex
            );
        }
    }

    public async Task<CategoryDto> ReplaceCategory(int categoryId, CategoryDto categoryDto) {
        return await UpdateCategory(categoryId, categoryDto);
    }

    public async Task<CategoryDto> DeleteCategory(int categoryId) {
        ValidateId(categoryId);
        try {
            var categoryToDelete = await _categoryRepository.GetCategory(categoryId);
            if (categoryToDelete is null) {
                throw new NotFoundException($"Category with id {categoryId} was not found.");
            }

            var deleted = await _categoryRepository.DeleteCategory(categoryId);
            if (!deleted) {
                throw new InfrastructureException("Could not persist category deletion.");
            }

            return _mapper.Map<CategoryDto>(categoryToDelete);
        } catch (AppException) {
            throw;
        } catch (Exception ex) {
            _logger.LogError(ex, "Error deleting category");
            throw new InfrastructureException(
                "An unexpected error occurred while deleting the category.",
                ex
            );
        }
    }

    private async Task ValidateCreateRequestAsync(CategoryCreateDto categoryDto) {
        if (categoryDto is null) {
            throw new BadRequestException("Category payload is required.");
        }

        var categoryName = categoryDto.Name?.Trim();
        if (string.IsNullOrWhiteSpace(categoryName)) {
            throw new BadRequestException("Category name is required.");
        }

        if (await _categoryRepository.ExistsCategoryName(categoryName)) {
            throw new ConflictException(
                $"The name '{categoryDto.Name}' is already in our records. Please use a different category name."
            );
        }
    }

    private async Task ValidateUpdateRequestAsync(int categoryId, CategoryDto categoryDto) {
        ValidateId(categoryId);

        if (categoryDto is null) {
            throw new BadRequestException("Category payload is required.");
        }

        if (categoryDto.Id > 0 && categoryDto.Id != categoryId) {
            throw new BadRequestException(
                $"Route id '{categoryId}' must match body id '{categoryDto.Id}'."
            );
        }

        var categoryName = categoryDto.Name?.Trim();
        if (string.IsNullOrWhiteSpace(categoryName)) {
            throw new BadRequestException("Category name is required.");
        }

        var currentCategory = await _categoryRepository.GetCategory(categoryId);
        if (currentCategory is null) {
            throw new NotFoundException($"Category with id {categoryId} was not found.");
        }

        var isDuplicatedName = await _categoryRepository.ExistsCategoryName(categoryName)
            && !string.Equals(currentCategory.Name, categoryName, StringComparison.OrdinalIgnoreCase);
        if (isDuplicatedName) {
            throw new ConflictException(
                $"The name '{categoryDto.Name}' is already in our records. Please use a different category name."
            );
        }
    }

    private Category MapCreateDtoToCategory(CategoryCreateDto dto) {
        var category = _mapper.Map<Category>(dto);
        category.Name = dto.Name.Trim();
        return category;
    }

    private Category MapUpdateDtoToCategory(int categoryId, CategoryDto dto) {
        var category = _mapper.Map<Category>(dto);
        category.Id = categoryId;
        category.Name = dto.Name.Trim();
        return category;
    }

    private static void ValidateId(int categoryId) {
        if (categoryId <= 0) {
            throw new BadRequestException("categoryId must be greater than 0.");
        }
    }
}
