using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using FixMyCampus.Domain.Entities;

namespace FixMyCampus.Infrastructure.Persistence.Configurations;

public class TicketStatusHistoryConfiguration
    : IEntityTypeConfiguration<TicketStatusHistory>
{
    public void Configure(
        EntityTypeBuilder<TicketStatusHistory> builder)
    {
        builder.ToTable("TicketStatusHistories");

        // Primary Key
        builder.HasKey(h => h.Id);

        // Ticket Id
        builder.Property(h => h.TicketId)
            .IsRequired();

        // Old Status
        builder.Property(h => h.OldStatus)
            .IsRequired()
            .HasConversion<int>();

        // New Status
        builder.Property(h => h.NewStatus)
            .IsRequired()
            .HasConversion<int>();

        // Changed By
        builder.Property(h => h.ChangedById)
            .IsRequired();

        // Changed At
        builder.Property(h => h.ChangedAt)
            .IsRequired();

        // Note
        builder.Property(h => h.Note)
            .HasMaxLength(1000)
            .IsRequired(false);



        builder.HasOne(h => h.Ticket)
            .WithMany(t => t.StatusHistory)
            .HasForeignKey(h => h.TicketId)
            .OnDelete(DeleteBehavior.Cascade);

        

        builder.HasOne(h => h.ChangedBy)
            .WithMany(u => u.StatusChanges)
            .HasForeignKey(h => h.ChangedById)
            .OnDelete(DeleteBehavior.Restrict);

       

        builder.HasIndex(h => h.TicketId);

        builder.HasIndex(h => h.ChangedById);

        builder.HasIndex(h => h.ChangedAt);
    }
}