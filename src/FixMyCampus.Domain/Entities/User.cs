


using FixMyCampus.Domain.Enums;

namespace FixMyCampus.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;

    public UserRole Role { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Tickets reported by this user
    public ICollection<Ticket> ReportedTickets { get; set; }
        = new List<Ticket>();

    // Tickets assigned to this user as technician
    public ICollection<Ticket> AssignedTickets { get; set; }
        = new List<Ticket>();

    // Status changes made by this user
    public ICollection<TicketStatusHistory> StatusChanges { get; set; }
        = new List<TicketStatusHistory>();
}