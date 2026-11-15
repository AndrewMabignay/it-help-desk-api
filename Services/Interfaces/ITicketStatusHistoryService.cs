using HelpDeskSystem.DTOs.TicketStatusHistories;

namespace HelpDeskSystem.Services.Interfaces;

public interface ITicketStatusHistoryService
{
    Task<IEnumerable<TicketStatusHistoryResponseDto>> GetByTicketIdAsync(
        Guid ticketId,
        string userId,
        CancellationToken cancellationToken);
}
