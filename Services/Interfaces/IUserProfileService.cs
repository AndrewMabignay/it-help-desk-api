using HelpDeskSystem.DTOs.UserProfiles;

namespace HelpDeskSystem.Services.Interfaces;

public interface IUserProfileService
{
    Task<IReadOnlyList<UserProfileResponseDto>> GetAllAsync(CancellationToken cancellationToken);

    Task<UserProfileResponseDto?> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken);

    Task<UserProfileResponseDto?> GetByUserIdAsync(
        string userId,
        CancellationToken cancellationToken);

    Task<UserProfileResponseDto> CreateAsync(
        CreateUserProfileRequestDto request,
        CancellationToken cancellationToken);

    Task<UserProfileResponseDto?> UpdateAsync(
        Guid id,
        UpdateUserProfileRequestDto request,
        CancellationToken cancellationToken);
}
