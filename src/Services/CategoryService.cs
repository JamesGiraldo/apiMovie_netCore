using ApiMovies.Interfaces.Repositories;
using ApiMovies.Interfaces.Services;
using ApiMovies.Models.Entities;
using ApiMovies.Models.Dtos;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace ApiMovies.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _ctRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<CategoryService> _logger;

    public CategoryService(
        ICategoryRepository ctRepo,
        IMapper mapper,
        ILogger<CategoryService> logger
    ) {
        _ctRepo = ctRepo;
        _mapper = mapper;
        _logger = logger;
    }

    public ServiceResult<IEnumerable<CategoryDto>> GetCategories(string? search = null) {
        try {
            var categories = _ctRepo.GetCategories(search);
            if (categories.Count == 0) {
                return FailureList(
                    "NotFound",
                    "Categories not found.",
                    string.IsNullOrWhiteSpace(search)
                        ? "No categories were found."
                        : $"No categories were found for search '{search}'."
                );
            }

            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return ServiceResult<IEnumerable<CategoryDto>>.Success(categoriesDto);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error getting categories");
            return FailureList(
                "Unexpected",
                "Could not retrieve categories.",
                "An unexpected error occurred while retrieving categories."
            );
        }
    }

    public ServiceResult<CategoryDto> GetCategory(int categoryId) {
        try {
            if (categoryId <= 0) {
                return Failure(
                    "InvalidId",
                    "Invalid category id.",
                    "categoryId must be greater than 0."
                );
            }

            var category = _ctRepo.GetCategory(categoryId);
            if (category is null) {
                return Failure(
                    "NotFound",
                    "Category not found.",
                    $"Category with id {categoryId} was not found."
                );
            }

            return ServiceResult<CategoryDto>.Success(_mapper.Map<CategoryDto>(category));
        } catch (Exception ex) {
            _logger.LogError(ex, "Error getting category");
            return Failure(
                "Unexpected",
                "Could not retrieve category.",
                "An unexpected error occurred while retrieving category."
            );
        }
    }

    public ServiceResult<CategoryDto> CreateCategory(CategoryCreateDto categoryDto) {
        var validation = ValidateCreateRequest(categoryDto);
        if (!validation.Succeeded) return validation;

        try {
            var category = MapCreateDtoToCategory(categoryDto);
            var created = _ctRepo.CreateCategory(category);
            if (!created) {
                return Failure(
                    "Persistence",
                    "Could not create category.",
                    "Could not persist category changes."
                );
            }

            return ServiceResult<CategoryDto>.Success(_mapper.Map<CategoryDto>(category));
        } catch (Exception ex) {
            _logger.LogError(ex, "Error creating category");
            return Failure(
                "Unexpected",
                "Could not create category.",
                "An unexpected error occurred while creating the category."
            );
        }
    }

    public ServiceResult<CategoryDto> UpdateCategory(int categoryId, CategoryDto categoryDto) {
        var validation = ValidateUpdateRequest(categoryId, categoryDto);
        if (!validation.Succeeded) return validation;

        try {
            var category = MapUpdateDtoToCategory(categoryId, categoryDto);
            var updated = _ctRepo.UpdateCategory(category);
            if (!updated) {
                return Failure(
                    "Persistence",
                    "Could not update category.",
                    "Could not persist category changes."
                );
            }

            return ServiceResult<CategoryDto>.Success(_mapper.Map<CategoryDto>(category));
        } catch (Exception ex) {
            _logger.LogError(ex, "Error updating category");
            return Failure(
                "Unexpected",
                "Could not update category.",
                "An unexpected error occurred while updating the category."
            );
        }
    }

    public ServiceResult<CategoryDto> ReplaceCategory(int categoryId, CategoryDto categoryDto) {
        var validation = ValidateUpdateRequest(categoryId, categoryDto);
        if (!validation.Succeeded) return validation;

        try {
            var category = MapUpdateDtoToCategory(categoryId, categoryDto);
            var replaced = _ctRepo.UpdateCategory(category);
            if (!replaced) {
                return Failure(
                    "Persistence",
                    "Could not replace category changes.",
                    "Could not persist category changes."
                );
            }

            return ServiceResult<CategoryDto>.Success(_mapper.Map<CategoryDto>(category));
        } catch (Exception ex) {
            _logger.LogError(ex, "Error replacing category");
            return Failure(
                "Unexpected",
                "Could not replace category changes.",
                "An unexpected error occurred while replacing the category changes."
            );
        }
    }

    public ServiceResult<CategoryDto> DeleteCategory(int categoryId) {
        var validation = ValidateDeleteRequest(categoryId);
        if (!validation.Succeeded) return validation;

        try {
            var categoryToDelete = _ctRepo.GetCategory(categoryId);
            if (categoryToDelete is null) {
                return Failure(
                    "NotFound",
                    "Category not found.",
                    $"Category with id {categoryId} was not found."
                );
            }

            var deleted = _ctRepo.DeleteCategory(categoryId);
            if (!deleted) {
                return Failure(
                    "Persistence",
                    "Could not delete category.",
                    "Could not persist category deletion."
                );
            }

            return ServiceResult<CategoryDto>.Success(_mapper.Map<CategoryDto>(categoryToDelete));
        } catch (Exception ex) {
            _logger.LogError(ex, "Error deleting category");
            return Failure(
                "Unexpected",
                "Could not delete category.",
                "An unexpected error occurred while deleting the category."
            );
        }
    }

    private ServiceResult<CategoryDto> ValidateCreateRequest(CategoryCreateDto categoryDto) {
        if (categoryDto is null) {
            return Failure(
                "InvalidPayload",
                "Invalid request payload.",
                "Category payload is required."
            );
        }

        var categoryName = categoryDto.Name?.Trim();
        if (string.IsNullOrWhiteSpace(categoryName)) {
            return Failure(
                "InvalidName",
                "Invalid category name.",
                "Category name is required."
            );
        }

        if (_ctRepo.ExistsCategoryName(categoryName)) {
            return Failure(
                "DuplicateName",
                "Category name already exists.",
                $"The name '{categoryDto.Name}' is already in our records. Please use a different category name."
            );
        }

        return ServiceResult<CategoryDto>.Success(default!);
    }

    private ServiceResult<CategoryDto> ValidateUpdateRequest(int categoryId, CategoryDto categoryDto) {
        if (categoryId <= 0) {
            return Failure(
                "InvalidId",
                "Invalid category id.",
                "categoryId must be greater than 0."
            );
        }

        if (categoryDto is null) {
            return Failure(
                "InvalidPayload",
                "Invalid request payload.",
                "Category payload is required."
            );
        }

        if (categoryDto.Id > 0 && categoryDto.Id != categoryId) {
            return Failure(
                "RouteBodyIdMismatch",
                "Route id and body id do not match.",
                $"Route id '{categoryId}' must match body id '{categoryDto.Id}'."
            );
        }

        var categoryName = categoryDto.Name?.Trim();
        if (string.IsNullOrWhiteSpace(categoryName)) {
            return Failure(
                "InvalidName",
                "Invalid category name.",
                "Category name is required."
            );
        }

        var currentCategory = _ctRepo.GetCategory(categoryId);
        if (currentCategory is null) {
            return Failure(
                "NotFound",
                "Category not found.",
                $"Category with id {categoryId} was not found."
            );
        }

        var isDuplicatedName = _ctRepo.ExistsCategoryName(categoryName)
            && !string.Equals(currentCategory.Name, categoryName, StringComparison.OrdinalIgnoreCase);
        if (isDuplicatedName) {
            return Failure(
                "DuplicateName",
                "Category name already exists.",
                $"The name '{categoryDto.Name}' is already in our records. Please use a different category name."
            );
        }

        return ServiceResult<CategoryDto>.Success(default!);
    }

    private ServiceResult<CategoryDto> ValidateDeleteRequest(int categoryId) {
        if (categoryId <= 0) {
            return Failure(
                "InvalidId",
                "Invalid category id.",
                "categoryId must be greater than 0."
            );
        }

        var currentCategory = _ctRepo.GetCategory(categoryId);
        if (currentCategory is null) {
            return Failure(
                "NotFound",
                "Category not found.",
                $"Category with id {categoryId} was not found."
            );
        }

        return ServiceResult<CategoryDto>.Success(default!);
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

    private static ServiceResult<CategoryDto> Failure(
        string code,
        string title,
        string detail
    ) {
        return ServiceResult<CategoryDto>.Failure(code, title, detail);
    }

    private static ServiceResult<IEnumerable<CategoryDto>> FailureList(
        string code,
        string title,
        string detail
    ) {
        return ServiceResult<IEnumerable<CategoryDto>>.Failure(code, title, detail);
    }
}
