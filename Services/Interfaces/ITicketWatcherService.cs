using HelpDeskSystem.DTOs.TicketWatchers;

namespace HelpDeskSystem.Services.Interfaces;

public interface ITicketWatcherService
{
    Task<IEnumerable<TicketWatcherResponseDto>> GetByTicketIdAsync(
        Guid ticketId,
        string userId,
        CancellationToken cancellationToken);

    Task<TicketWatcherResponseDto> AddAsync(
        Guid ticketId,
        CreateTicketWatcherDto request,
        string userId,
        CancellationToken cancellationToken);

    Task RemoveAsync(
        Guid ticketId,
        string watcherUserId,
        string userId,
        CancellationToken cancellationToken);
}
