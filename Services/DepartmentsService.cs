using HelpDeskSystem.Data;
using HelpDeskSystem.DTOs.Departments;
using HelpDeskSystem.Models.Entities;
using HelpDeskSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Services;

public sealed class DepartmentsService : IDepartmentsService
{
    private readonly ApplicationDbContext _dbContext;

    public DepartmentsService(ApplicationDbContext dbContext)
    { 
        _dbContext = dbContext;
    }

    // Get all departments
    public async Task<IReadOnlyList<DepartmentResponseDto>> GetAllAsync(
        bool includeInactive,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Departments
            .AsNoTracking();

        if (!includeInactive)
        {
            query = query.Where(department => department.IsActive);
        }

        return await query
            .OrderBy(department => department.Name)
            .Select(department => new DepartmentResponseDto 
            { 
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt,
                UpdatedAt = department.UpdatedAt
            })
            .ToListAsync(cancellationToken);
    }

    // Get department by ID
    public async Task<DepartmentResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken)
    { 
        return await _dbContext.Departments
            .AsNoTracking()
            .Where(department => department.Id == id)
            .Select(department => new DepartmentResponseDto 
            { 
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                IsActive = department.IsActive,
                CreatedAt = department.CreatedAt,
                UpdatedAt = department.UpdatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    // Create department
    public async Task<DepartmentResponseDto> CreateAsync(
        CreateDepartmentRequestDto request,
        CancellationToken cancellationToken)
    {
        var name = request.Name.Trim();

        var existingDepartment = await _dbContext.Departments
            .AnyAsync(
                department => department.Name == name,
                cancellationToken);

        if (existingDepartment)
        {
            throw new InvalidOperationException("Department name already exists.");
        }

        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = request.Description?.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _dbContext.Departments.Add(department);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(department);
    }

    // Update department
    public async Task<DepartmentResponseDto?> UpdateAsync(
        Guid id,
        UpdateDepartmentRequestDto request,
        CancellationToken cancellationToken)
    { 
        var department = await _dbContext.Departments
            .FirstOrDefaultAsync(
                department => department.Id == id,
                cancellationToken);

        if (department is null)
        {
            return null;
        }

        var name = request.Name.Trim();

        var existingDepartment = await _dbContext.Departments
            .AnyAsync(
                existing =>
                    existing.Name == name &&
                    existing.Id != id,
                cancellationToken);

        if (existingDepartment)
        {
            throw new InvalidOperationException("Department name already exists.");
        }

        department.Name = name;
        department.Description = request.Description?.Trim();
        department.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return MapToResponse(department);
    }

    // Update department status
    public async Task<bool> UpdateStatusAsync(
        Guid id,
        UpdateDepartmentStatusRequestDto request,
        CancellationToken cancellationToken)
    {
        var department = await _dbContext.Departments
            .FirstOrDefaultAsync(
                department => department.Id == id,
                cancellationToken);

        if (department is null)
        {
            return false;
        }

        department.IsActive = request.IsActive;
        department.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    // Map entity to response DTO
    private static DepartmentResponseDto MapToResponse(Department department)
    {
        return new DepartmentResponseDto
        {
            Id = department.Id,
            Name = department.Name,
            Description = department.Description,
            IsActive = department.IsActive,
            CreatedAt = department.CreatedAt,
            UpdatedAt = department.UpdatedAt
        };
    }
}
