using HelpDeskSystem.DTOs.Tickets;
using HelpDeskSystem.Models.Entities;

namespace HelpDeskSystem.Mappings;

public static class TicketMapping
{
    public static TicketResponseDto ToResponseDto(this Ticket ticket)
    {
        return new TicketResponseDto
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            Subject = ticket.Subject,
            Description = ticket.Description,

            CategoryId = ticket.CategoryId,
            CategoryName = ticket.Category.Name,

            PriorityId = ticket.PriorityId,
            PriorityName = ticket.Priority.Name,

            RequesterId = ticket.RequesterId,
            RequesterName = $"{ticket.Requester.FirstName} {ticket.Requester.LastName}",
            
            AssignedToId = ticket.AssignedToId,
            AssignedToName = ticket.AssignedTo == null
                ? null
                : $"{ticket.AssignedTo.FirstName} {ticket.AssignedTo.LastName}",

            Status = ticket.Status,

            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            ResolvedAt = ticket.ResolvedAt,
            ClosedAt = ticket.ClosedAt
        };
    }
}
