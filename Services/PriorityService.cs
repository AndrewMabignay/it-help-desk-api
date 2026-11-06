using HelpDeskSystem.Data;
using HelpDeskSystem.DTOs;
using HelpDeskSystem.DTOs.Categories;
using HelpDeskSystem.DTOs.Priorities;
using HelpDeskSystem.Models.Entities;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Services;

public sealed class PriorityService : IPriorityService
{
    private readonly ApplicationDbContext _dbContext;

    public PriorityService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Get all priorities (server-side paginated and search)
    public async Task<PagedResponseDto<PriorityResponseDto>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        bool includeInactive,
        CancellationToken cancellationToken)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _dbContext.Priorities.AsNoTracking();

        // Search by priority name
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(priority =>
                priority.Name.Contains(searchTerm));
        }

        // Filter inactive priorities
        if (!includeInactive)
        {
            query = query.Where(priority => priority.IsActive);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var priorities = await query
            .OrderBy(priority => priority.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(priority => new PriorityResponseDto
            {
                Id = priority.Id,
                Name = priority.Name,
                Description = priority.Description,
                Level = priority.Level,
                IsActive = priority.IsActive,
                CreatedAt = priority.CreatedAt,
                UpdatedAt = priority.UpdatedAt
            })
            .ToListAsync();

        return new PagedResponseDto<PriorityResponseDto>
        {
            Items = priorities,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    // Get priority by ID
    public async Task<PriorityResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    {
        return await _dbContext.Priorities
            .AsNoTracking()
            .Where(priority => priority.Id == id)
            .Select(priority => new PriorityResponseDto
            {
                Id = priority.Id,
                Name = priority.Name,
                Description = priority.Description,
                Level = priority.Level,
                IsActive = priority.IsActive,
                CreatedAt = priority.CreatedAt,
                UpdatedAt = priority.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    // Create priority
    public async Task<PriorityResponseDto> CreateAsync(
        CreatePriorityRequestDto request,
        CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();

        // Check for an existing priority.
        var existingPriority = await _dbContext.Priorities
            .AnyAsync(
                priority => priority.Name == name,
                cancellationToken);

        if (existingPriority) throw new InvalidOperationException("Priority name already exists.");

        var priority = new Priority
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = request.Description?.Trim(),
            Level = request.Level,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Priorities.Add(priority);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(priority);
    }

    // Update priority
    public async Task<PriorityResponseDto?> UpdateAsync(
        Guid id,
        UpdatePriorityRequestDto request,
        CancellationToken cancellationToken)
    {
        var priority = await _dbContext.Priorities
            .FirstOrDefaultAsync(
                priority => priority.Id == id,
                cancellationToken);

        if (priority is null) return null;


        var name = request.Name.Trim();

        var existingPriority = await _dbContext.Priorities
            .AnyAsync(
                existing =>
                    existing.Name == name &&
                    existing.Id != id,
                cancellationToken);

        if (existingPriority)
        {
            throw new InvalidOperationException("Priority name already exists.");
        }

        priority.Name = name;
        priority.Description = request.Description?.Trim();
        priority.Level = request.Level;
        priority.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(priority);
    }

    // Update priority status
    public async Task<bool> UpdateStatusAsync(
        Guid id,
        UpdatePriorityStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var priority = await _dbContext.Priorities
            .FirstOrDefaultAsync(
                priority => priority.Id == id,
                cancellationToken);

        if (priority is null)
        {
            return false;
        }

        priority.IsActive = request.IsActive;
        priority.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    // Map entity to response DTO
    private static PriorityResponseDto MapToResponse(Priority priority)
    {
        return new PriorityResponseDto
        {
            Id = priority.Id,
            Name = priority.Name,
            Description = priority.Description,
            IsActive = priority.IsActive,
            Level = priority.Level,
            CreatedAt = priority.CreatedAt,
            UpdatedAt = priority.UpdatedAt
        };
    }
}
