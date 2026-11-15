using HelpDeskSystem.DTOs.TicketStatusHistories;
using HelpDeskSystem.Models.Entities;

namespace HelpDeskSystem.Mappings;

public static class TicketStatusHistoryMapping
{
    public static TicketStatusHistoryResponseDto ToResponseDto(this TicketStatusHistory history)
    {
        return new TicketStatusHistoryResponseDto
        {
            Id = history.Id,
            TicketId = history.TicketId,
            ChangedById = history.ChangedById,
            ChangedByName = history.ChangedBy != null
                ? $"{history.ChangedBy.FirstName} {history.ChangedBy.LastName}"
                : null,
            OldStatus = history.OldStatus?.ToString(),
            NewStatus = history.NewStatus.ToString(),
            Remarks = history.Remarks,
            ChangedAt = history.ChangedAt
        };
    }
}
