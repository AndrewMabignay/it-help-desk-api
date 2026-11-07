namespace HelpDeskSystem.Models.Entities
{
    public class TicketComment
    {
        public int Id { get; set; }

        // Foreign Keys
        public Guid TicketId { get; set; }

        public string UserId { get; set; } = string.Empty;

        // Comment Information
        public string Comment { get; set; } = string.Empty;

        public bool IsInterval { get; set; } = false;

        // Navigation Properties
        public Ticket Ticket { get; set; } = null!;

        public ApplicationUser User { get; set; } = null!;

        // Timestamp
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
