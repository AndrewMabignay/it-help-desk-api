using HelpDeskSystem.DTOs.TicketAttachments;
using HelpDeskSystem.Models.Entities;

namespace HelpDeskSystem.Mappings;

public static class TicketAttachmentMapping
{
    public static TicketAttachmentResponseDto ToResponseDto(this TicketAttachment attachment)
    {
        return new TicketAttachmentResponseDto
        {
            Id = attachment.Id,
            TicketId = attachment.TicketId,
            UploadedById = attachment.UploadedById,
            UploadedByName = $"{attachment.UploadedBy.FirstName} {attachment.UploadedBy.LastName}",
            OriginalFileName = attachment.OriginalFileName,
            ContentType = attachment.ContentType,
            FileSizeBytes = attachment.FileSizeBytes,
            UploadedAt = attachment.UploadedAt
        };
    }
}
