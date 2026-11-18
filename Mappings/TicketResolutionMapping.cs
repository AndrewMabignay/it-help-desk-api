using HelpDeskSystem.DTOs.TicketResolutions;
using HelpDeskSystem.Models.Entities;

namespace HelpDeskSystem.Mappings;

public static class TicketResolutionMapping
{
    public static TicketResolutionResponseDto ToResponseDto(
        this TicketResolution resolution) 
    {
        return new TicketResolutionResponseDto
        {
            id = resolution.Id,
            TicketId = resolution.TicketId,
            ResolvedById = resolution.ResolvedById,

            ResolvedByName = resolution.ResolvedById != null
                ? $"{resolution.ResolvedBy.FirstName} {resolution.ResolvedBy.LastName}"
                : string.Empty,

            ResolutionSummary = resolution.ResolutionSummary,
            ResolutionDetails = resolution.ResolutionDetails,
            ResolvedAt = resolution.ResolvedAt
        };
    }
}
