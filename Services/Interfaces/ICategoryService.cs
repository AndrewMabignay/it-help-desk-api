using HelpDeskSystem.DTOs;
using HelpDeskSystem.DTOs.Categories;

namespace HelpDeskSystem.Services.Interfaces;

public interface ICategoryService
{
    Task<PagedResponseDto<CategoryResponseDto>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        bool includeInactive,
        CancellationToken cancellationToken);

    Task<CategoryResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<CategoryResponseDto> CreateAsync(
        CreateCategoryRequestDto request,
        CancellationToken cancellationToken);

    Task<CategoryResponseDto?> UpdateAsync(
        Guid id,
        UpdateCategoryRequestDto request,
        CancellationToken cancellationToken);

    Task<bool> UpdateStatusAsync(
        Guid id,
        UpdateCategoryStatusRequestDto request,
        CancellationToken cancellationToken);
}
