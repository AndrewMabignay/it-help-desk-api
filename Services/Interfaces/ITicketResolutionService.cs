using HelpDeskSystem.DTOs.TicketResolutions;

namespace HelpDeskSystem.Services.Interfaces
{
    public interface ITicketResolutionService
    {
        Task<TicketResolutionResponseDto> CreateAsync(
            Guid ticketId,
            CreateTicketResolutionDto request,
            string resolvedById,
            CancellationToken cancellationToken);

        Task<TicketResolutionResponseDto?> GetByTicketIdAsync(
            Guid ticketId,
            string userId,
            CancellationToken cancellationToken);
    }
}
