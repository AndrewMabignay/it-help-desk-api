using HelpDeskSystem.DTOs.Auth;
using System.Security.Claims;

namespace HelpDeskSystem.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(
            LoginRequestDto loginRequestDto, 
            CancellationToken cancellationToken);

        Task<CurrentUserDto> GetCurrentUserAsync(
            ClaimsPrincipal principal,
            CancellationToken cancellationToken);

        Task LogoutAsync(
            ClaimsPrincipal principal,
            CancellationToken cancellationToken);
    }
}
