using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FixMyCampus.Domain.Entities;

namespace FixMyCampus.Infrastructure.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.ToTable("Tickets");

        // Primary Key
        builder.HasKey(t => t.Id);

        // Ticket Number
        builder.Property(t => t.TicketNumber)
            .IsRequired()
            .HasMaxLength(30);

        // Unique Ticket Number
        builder.HasIndex(t => t.TicketNumber)
            .IsUnique();

        // Category
        builder.Property(t => t.Category)
            .IsRequired()
            .HasConversion<int>();

        // Building
        builder.Property(t => t.Building)
            .IsRequired()
            .HasConversion<int>();

        // Room
        builder.Property(t => t.Room)
            .IsRequired()
            .HasMaxLength(50);

        // Description
        builder.Property(t => t.Description)
            .IsRequired()
            .HasMaxLength(2000);

        // Priority
        builder.Property(t => t.Priority)
            .IsRequired()
            .HasConversion<int>();

        // Status
        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<int>();

        // Reporter
        builder.Property(t => t.ReporterId)
            .IsRequired();

        // Technician
        builder.Property(t => t.TechnicianId)
            .IsRequired(false);

        builder.Property(t => t.TechnicianName)
            .IsRequired(false)
            .HasMaxLength(150);

        // Created At
        builder.Property(t => t.CreatedAt)
            .IsRequired();

        // Updated At
        builder.Property(t => t.UpdatedAt)
            .IsRequired(false);

        // Resolved At
        builder.Property(t => t.ResolvedAt)
            .IsRequired(false);

        

        builder.HasOne(t => t.Reporter)
            .WithMany(u => u.ReportedTickets)
            .HasForeignKey(t => t.ReporterId)
            .OnDelete(DeleteBehavior.Restrict);

    

        builder.HasOne(t => t.Technician)
            .WithMany(u => u.AssignedTickets)
            .HasForeignKey(t => t.TechnicianId)
            .OnDelete(DeleteBehavior.Restrict);

     

        builder.HasMany(t => t.StatusHistory)
            .WithOne(h => h.Ticket)
            .HasForeignKey(h => h.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

       

        builder.HasIndex(t => t.ReporterId);

        builder.HasIndex(t => t.TechnicianId);

        builder.HasIndex(t => t.Status);

        builder.HasIndex(t => t.Category);

        builder.HasIndex(t => t.CreatedAt);
    }
}