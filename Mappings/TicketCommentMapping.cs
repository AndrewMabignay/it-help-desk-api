using HelpDeskSystem.DTOs.TicketComments;
using HelpDeskSystem.Models.Entities;

namespace HelpDeskSystem.Mappings;

public static class TicketCommentMapping
{
    public static TicketCommentResponseDto ToResponseDto(this TicketComment comment)
    {
        return new TicketCommentResponseDto
        {
            Id = comment.Id,
            TicketId = comment.TicketId,
            UserId = comment.UserId,
            UserName = $"{comment.User.FirstName} {comment.User.LastName}",
            Comment = comment.Comment,
            IsInternal = comment.IsInternal,
            CreatedAt = comment.CreatedAt
        };
    }
}
