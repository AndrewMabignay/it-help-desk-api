using HelpDeskSystem.DTOs.Departments;

namespace HelpDeskSystem.Services.Interfaces
{
    public interface IDepartmentsService
    {
        Task<IReadOnlyList<DepartmentResponseDto>> GetAllAsync(
            bool includeInactive,
            CancellationToken cancellationToken);

        Task<DepartmentResponseDto?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<DepartmentResponseDto> CreateAsync(
            CreateDepartmentRequestDto request,
            CancellationToken cancellationToken);

        Task<DepartmentResponseDto?> UpdateAsync(
            Guid id,
            UpdateDepartmentRequestDto request,
            CancellationToken cancellationToken);

        Task<bool> UpdateStatusAsync(
            Guid id,
            UpdateDepartmentStatusRequestDto requust,
            CancellationToken cancellationToken);
    }
}
