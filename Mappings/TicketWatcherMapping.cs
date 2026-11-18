using HelpDeskSystem.DTOs.TicketWatchers;
using HelpDeskSystem.Models.Entities;

namespace HelpDeskSystem.Mappings;

public static class TicketWatcherMapping
{
    public static TicketWatcherResponseDto ToResponseDto(
        this TicketWatcher watcher)
    {
        return new TicketWatcherResponseDto
        { 
            Id = watcher.Id,
            TicketId = watcher.TicketId,
            UserId = watcher.UserId,
            UserName = $"{watcher.User.FirstName} {watcher.User.LastName}",
            CreatedAt = watcher.CreatedAt
        };
    }
}
