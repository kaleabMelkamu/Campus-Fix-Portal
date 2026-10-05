using FixMyCampus.Domain.Enums;
using FixMyCampus.Domain.Exceptions;

namespace FixMyCampus.Domain.Entities;

public class Ticket
{
    public Guid Id { get; set; } = Guid.NewGuid();

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

    public string? TechnicianName { get; set; }

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

    /// <summary>
    /// Hard Rule: Assign is what moves ticket from New -> Assigned.
    /// Requires technician name.
    /// </summary>
    public void AssignTechnician(string technicianName, Guid? technicianId, Guid changedById, string? note = null)
    {
        if (Status != TicketStatus.New)
        {
            throw new DomainException($"Cannot assign technician. Ticket must be in 'New' status, but is currently '{Status}'.");
        }

        if (string.IsNullOrWhiteSpace(technicianName))
        {
            throw new DomainException("Technician name is required to assign a ticket.");
        }

        var oldStatus = Status;
        Status = TicketStatus.Assigned;
        TechnicianName = technicianName.Trim();
        TechnicianId = technicianId;
        UpdatedAt = DateTime.UtcNow;

        StatusHistory.Add(new TicketStatusHistory
        {
            Id = Guid.NewGuid(),
            TicketId = Id,
            OldStatus = oldStatus,
            NewStatus = Status,
            ChangedById = changedById,
            ChangedAt = DateTime.UtcNow,
            Note = note ?? $"Assigned to technician: {TechnicianName}"
        });
    }

    /// <summary>
    /// Hard Rule: Status only moves forward: New -> Assigned -> InProgress -> Resolved.
    /// Cannot skip steps, cannot move backwards.
    /// </summary>
    public void MoveStatus(TicketStatus targetStatus, Guid changedById, string? note = null)
    {
        if (Status == targetStatus)
        {
            throw new DomainException($"Ticket is already in status '{Status}'.");
        }

        // Backward check
        if ((int)targetStatus < (int)Status)
        {
            throw new DomainException($"Illegal status move: Status can only move forward, cannot go from '{Status}' to '{targetStatus}'.");
        }

        // Exact forward step checks
        if (Status == TicketStatus.New)
        {
            throw new DomainException("Illegal status skip: Ticket in 'New' status must be assigned to a technician first (New -> Assigned).");
        }

        if (Status == TicketStatus.Assigned && targetStatus != TicketStatus.InProgress)
        {
            throw new DomainException($"Illegal status skip: Ticket must move to 'InProgress' before it can be resolved (Assigned -> InProgress).");
        }

        if (Status == TicketStatus.InProgress && targetStatus != TicketStatus.Resolved)
        {
            throw new DomainException($"Invalid status transition from '{Status}' to '{targetStatus}'.");
        }

        if (Status == TicketStatus.Resolved)
        {
            throw new DomainException("Ticket is already 'Resolved' and cannot be modified further.");
        }

        var oldStatus = Status;
        Status = targetStatus;
        UpdatedAt = DateTime.UtcNow;

        if (targetStatus == TicketStatus.Resolved)
        {
            ResolvedAt = DateTime.UtcNow;
        }

        StatusHistory.Add(new TicketStatusHistory
        {
            Id = Guid.NewGuid(),
            TicketId = Id,
            OldStatus = oldStatus,
            NewStatus = Status,
            ChangedById = changedById,
            ChangedAt = DateTime.UtcNow,
            Note = note ?? $"Status advanced from {oldStatus} to {Status}"
        });
    }
}
