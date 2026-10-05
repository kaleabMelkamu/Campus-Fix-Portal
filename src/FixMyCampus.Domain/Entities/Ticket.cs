
using FixMyCampus.Domain.Enums;


namespace FixMyCampus.Domain.Entities;

public class Ticket
{
    public Guid Id { get; set; }

    public string TicketNumber { get; set; } = string.Empty;

    public TicketCategory Category { get; set; }

    public Building Building { get; set; }

    public string Room { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ComplaintPriority Priority { get; set; } = ComplaintPriority.Medium;

    public TicketStatus Status { get; set; } = TicketStatus.New;

    // Reporter
    public Guid ReporterId { get; set; }

    // Technician
    public Guid? TechnicianId { get; set; }

    // Dates
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public DateTime? ResolvedAt { get; set; }

    // Navigation: Reporter
    public User Reporter { get; set; } = null!;

    // Navigation: Assigned Technician
    public User? Technician { get; set; }

    // Navigation: Status History
    public ICollection<TicketStatusHistory> StatusHistory { get; set; }
        = new List<TicketStatusHistory>();
}
