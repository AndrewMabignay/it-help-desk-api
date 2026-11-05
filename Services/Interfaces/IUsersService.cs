using HelpDeskSystem.DTOs.Users;

namespace HelpDeskSystem.Services.Interfaces;

public interface IUsersService
{
    Task<PagedResponseDto<UserResponseDto>> GetUsersAsynch(
        int pageNumber,
        int pageSize,
        string? searchTerm,
        CancellationToken cancellationToken);

    Task<UserResponseDto?> GetUserByIdAsync(
        string id,
        CancellationToken cancellationToken);

    Task<UserResponseDto?> CreateAsynch(
        CreateUserRequestDto createUserRequestDto,
        CancellationToken cancellationToken);

    Task<UserResponseDto?> UpdateAsync(
        string id,
        UpdateUserRequestDto updateUserRequestDto,
        CancellationToken cancellationToken);

    Task<bool> UpdateStatusAsync(
        string id,
        bool isActive,
        CancellationToken cancellationToken);   
}