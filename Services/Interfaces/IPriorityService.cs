using HelpDeskSystem.DTOs;
using HelpDeskSystem.DTOs.Priorities;

namespace HelpDeskSystem.Services.Interfaces;

public interface IPriorityService
{
    // Get all priorities
    Task<PagedResponseDto<PriorityResponseDto>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        bool includeInactive,
        CancellationToken cancellationToken);

    // Get priority by ID
    Task<PriorityResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    // Create priority
    Task<PriorityResponseDto> CreateAsync(
        CreatePriorityRequestDto request,
        CancellationToken cancellationToken);

    // Update priority
    Task<PriorityResponseDto?> UpdateAsync(
        Guid id,
        UpdatePriorityRequestDto request,
        CancellationToken cancellationToken);

    // Update priority status
    Task<bool> UpdateStatusAsync(
        Guid id,
        UpdatePriorityStatusRequestDto request,
        CancellationToken cancellationToken);
}
