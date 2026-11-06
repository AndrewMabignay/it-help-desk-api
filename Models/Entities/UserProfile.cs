namespace HelpDeskSystem.Models.Entities;

public class UserProfile
{
    public Guid Id { get; set; }

    // Foreign Key
    public string UserId { get; set; } = string.Empty;

    // Personal Information
    public string? EmployeeNumber { get; set; }
    
    public string? JobTitle { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ProfilePictureUrl { get; set; }

    // Department Relationship
    public Guid? DepartmentId { get; set; }

    public Department? Department { get; set; }

    // Account Information
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; }

    // Navigation Property 
    public ApplicationUser User { get; set; } = null!;
}
