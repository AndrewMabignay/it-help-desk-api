using HelpDeskSystem.Data;
using HelpDeskSystem.DTOs;
using HelpDeskSystem.DTOs.Categories;
using HelpDeskSystem.Models.Entities;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Services;

public sealed class CategoryService : ICategoryService
{
    private readonly ApplicationDbContext _dbContext;

    public CategoryService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Get all categories (server-side paginated and search)
    public async Task<PagedResponseDto<CategoryResponseDto>> GetAllAsync(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        bool includeInactive,
        CancellationToken cancellationToken)
    {
        pageNumber = Math.Max(1, pageNumber);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var query = _dbContext.Categories.AsNoTracking();

        // Search by category name
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(category => 
                category.Name.Contains(searchTerm));
        }

        // Filter inactive categories
        if (!includeInactive)
        {
            query = query.Where(category => category.IsActive);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var categories = await query
            .OrderBy(category => category.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(category => new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            })
            .ToListAsync();

        return new PagedResponseDto<CategoryResponseDto>
        { 
            Items = categories,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    // Get category by ID
    public async Task<CategoryResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken) 
    {
        return await _dbContext.Categories
            .AsNoTracking()
            .Where(category => category.Id == id)
            .Select(category => new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            })
            .FirstOrDefaultAsync();
    }

    // Create category
    public async Task<CategoryResponseDto> CreateAsync(
        CreateCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();

        // Check for an existing category.
        var existingCategory = await _dbContext.Categories
            .AnyAsync(
                category => category.Name == name,
                cancellationToken);

        if (existingCategory) throw new InvalidOperationException("Category name already exists.");

        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = request.Description?.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Categories.Add(category);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(category);
    }

    // Update category
    public async Task<CategoryResponseDto?> UpdateAsync(
        Guid id,
        UpdateCategoryRequestDto request,
        CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(
                category => category.Id == id,
                cancellationToken);

        if (category is null) return null;


        var name = request.Name.Trim();

        var existingCategory = await _dbContext.Categories
            .AnyAsync(
                existing =>
                    existing.Name == name &&
                    existing.Id != id,
                cancellationToken);

        if (existingCategory)
        {
            throw new InvalidOperationException("Category name already exists.");
        }

        category.Name = name;
        category.Description = request.Description?.Trim();
        category.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(category);
    }

    // Update category status
    public async Task<bool> UpdateStatusAsync(
        Guid id,
        UpdateCategoryStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var category = await _dbContext.Categories
            .FirstOrDefaultAsync(
                category => category.Id == id,
                cancellationToken);

        if (category is null)
        {
            return false;
        }

        category.IsActive = request.IsActive;
        category.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    // Map entity to response DTO
    private static CategoryResponseDto MapToResponse(Category category)
    {
        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            IsActive = category.IsActive,
            CreatedAt = category.CreatedAt,
            UpdatedAt = category.UpdatedAt
        };
    }
}
