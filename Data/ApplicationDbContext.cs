using HelpDeskSystem.Models;
using HelpDeskSystem.Models.Entities;
using HelpDeskSystem.Models.Entitiesl;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HelpDeskSystem.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options
    ) : base(options)
    {
    }

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    // Custom Tables
    public DbSet<Department> Departments => Set<Department>();

    public DbSet<UserProfile> UserProfiles => Set<UserProfile>();

    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Priority> Priorities => Set<Priority>();
    
    public DbSet<Ticket> Tickets => Set<Ticket>();

    public DbSet<TicketComment> TicketComments => Set<TicketComment>();

    public DbSet<TicketAttachment> TicketAttachments => Set<TicketAttachment>();
    
    public DbSet<TicketWatcher> TicketWatchers => Set<TicketWatcher>();

    public DbSet<TicketStatusHistory> TicketStatusHistories => Set<TicketStatusHistory>();
    
    public DbSet<TicketAssignment> TicketAssignments => Set<TicketAssignment>();

    // Entity Relationships
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly
        );

        // User Profile Configuration
        builder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(up => up.Id);

            entity.HasIndex(up => up.UserId)
                .IsUnique();

            entity.HasOne(up => up.User)
                .WithOne()
                .HasForeignKey<UserProfile>(up => up.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(up => up.Department)
                .WithMany()
                .HasForeignKey(up => up.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.Property(up => up.EmployeeNumber)
                .HasMaxLength(50);

            entity.Property(up => up.JobTitle)
                .HasMaxLength(199);
        });

        // Ticket Configuration
        builder.Entity<Ticket>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.HasIndex(t => t.TicketNumber)
                .IsUnique();

            entity.Property(t => t.TicketNumber)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(t => t.Subject)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(t => t.Description)
                .IsRequired();

            entity.HasOne(t => t.Category)
                .WithMany()
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Priority)
                .WithMany()
                .HasForeignKey(t => t.PriorityId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.Requester)
                .WithMany()
                .HasForeignKey(t => t.RequesterId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(t => t.AssignedTo)
                .WithMany()
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // TicketComment Configuration
        builder.Entity<TicketComment>(entity =>
        {
            entity.HasKey(tc => tc.Id);

            entity.Property(tc => tc.Comment)
                .IsRequired();

            entity.HasOne(tc => tc.Ticket)
                .WithMany()
                .HasForeignKey(tc => tc.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(tc => tc.User)
                .WithMany()
                .HasForeignKey(tc => tc.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });


        // TicketAttachment Configuration
        builder.Entity<TicketAttachment>(entity =>
        {
            entity.HasKey(ta => ta.Id);

            entity.Property(ta => ta.OriginalFileName)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(ta => ta.StoredFileName)
                .HasMaxLength(255)
                .IsRequired();

            entity.Property(ta => ta.FilePath)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(ta => ta.ContentType)
                .HasMaxLength(150)
                .IsRequired();

            entity.HasOne(ta => ta.Ticket)
                .WithMany()
                .HasForeignKey(ta => ta.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ta => ta.UploadedBy)
                .WithMany()
                .HasForeignKey(ta => ta.UploadedById)
                .OnDelete(DeleteBehavior.Restrict);
        });


        // TicketWatcher Configuration
        builder.Entity<TicketWatcher>(entity =>
        {
            entity.HasKey(tw => tw.Id);

            // Prevent duplicate watchers
            entity.HasIndex(tw => new { tw.TicketId, tw.UserId })
                .IsUnique();

            entity.HasOne(tw => tw.Ticket)
                .WithMany()
                .HasForeignKey(tw => tw.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(tw => tw.User)
                .WithMany()
                .HasForeignKey(tw => tw.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // TicketStatusHistory Configuration
        builder.Entity<TicketStatusHistory>(entity =>
        {
            entity.HasKey(tsh => tsh.Id);

            entity.Property(tsh => tsh.Remarks)
                .HasMaxLength(1000);

            entity.HasOne(tsh => tsh.Ticket)
                .WithMany()
                .HasForeignKey(tsh => tsh.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(tsh => tsh.ChangedBy)
                .WithMany()
                .HasForeignKey(tsh => tsh.ChangedById)
                .OnDelete(DeleteBehavior.Restrict);
        });


        // TicketAssignment Configuration
        builder.Entity<TicketAssignment>(entity =>
        {
            entity.HasKey(ta => ta.Id);

            entity.Property(ta => ta.Remarks)
                .HasMaxLength(1000);

            entity.HasOne(ta => ta.Ticket)
                .WithMany()
                .HasForeignKey(ta => ta.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ta => ta.PreviousAssignee)
                .WithMany()
                .HasForeignKey(ta => ta.PreviousAssigneeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ta => ta.AssignedTo)
                .WithMany()
                .HasForeignKey(ta => ta.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(ta => ta.AssignedBy)
                .WithMany()
                .HasForeignKey(ta => ta.AssignedById)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // TicketResolution Configuration
        builder.Entity<TicketResolution>(entity =>
        {
            entity.HasKey(tr => tr.Id);

            entity.Property(tr => tr.ResolutionSummary)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(tr => tr.ResolutionDetails)
                .HasMaxLength(2000);

            entity.HasOne(tr => tr.Ticket)
                .WithMany()
                .HasForeignKey(tr => tr.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(tr => tr.ResolvedBy)
                .WithMany()
                .HasForeignKey(tr => tr.ResolvedById)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
