

using FixMyCampus.Domain.Enums;

namespace FixMyCampus.Domain.Entities;

public class TicketStatusHistory
{
    public Guid Id { get; set; }

    public Guid TicketId { get; set; }

    public TicketStatus OldStatus { get; set; }

    public TicketStatus NewStatus { get; set; }

    public Guid ChangedById { get; set; }

    public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

    public string? Note { get; set; }

    // Ticket whose status changed
    public Ticket Ticket { get; set; } = null!;

    // User who changed the status
    public User ChangedBy { get; set; } = null!;
}