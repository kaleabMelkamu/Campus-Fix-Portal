using FixMyCampus.Domain.Enums;

namespace FixMyCampus.Application.DTOs;

public record TicketDto
{
    public Guid Id { get; init; }
    public string TicketNumber { get; init; } = string.Empty;
    public TicketCategory Category { get; init; }
    public string CategoryName => Category.ToString();
    public Building Building { get; init; }
    public string BuildingName => Building.ToString();
    public string Room { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public ComplaintPriority Priority { get; init; }
    public string PriorityName => Priority.ToString();
    public TicketStatus Status { get; init; }
    public string StatusName => Status.ToString();
    public Guid ReporterId { get; init; }
    public string ReporterName { get; init; } = string.Empty;
    public Guid? TechnicianId { get; init; }
    public string? TechnicianName { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public DateTime? ResolvedAt { get; init; }
}

public record TicketStatusHistoryDto
{
    public Guid Id { get; init; }
    public TicketStatus OldStatus { get; init; }
    public string OldStatusName => OldStatus.ToString();
    public TicketStatus NewStatus { get; init; }
    public string NewStatusName => NewStatus.ToString();
    public Guid ChangedById { get; init; }
    public string ChangedByName { get; init; } = string.Empty;
    public DateTime ChangedAt { get; init; }
    public string? Note { get; init; }
}

public record TicketDetailDto : TicketDto
{
    public List<TicketStatusHistoryDto> StatusHistory { get; init; } = new();
}

public record CreateTicketDto
{
    public TicketCategory Category { get; init; }
    public Building Building { get; init; }
    public string Room { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public ComplaintPriority Priority { get; init; } = ComplaintPriority.Medium;
}

public record AssignTicketDto
{
    public string TechnicianName { get; init; } = string.Empty;
    public Guid? TechnicianId { get; init; }
    public string? Note { get; init; }
}

public record UpdateTicketStatusDto
{
    public TicketStatus Status { get; init; }
    public string? Note { get; init; }
}

public record TicketFilterDto
{
    public Building? Building { get; init; }
    public TicketStatus? Status { get; init; }
    public string? Search { get; init; }
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
}

public record AdminStatsDto
{
    public int TotalTickets { get; init; }
    public int OpenTickets { get; init; }
    public int NewTickets { get; init; }
    public int AssignedTickets { get; init; }
    public int InProgressTickets { get; init; }
    public int ResolvedTickets { get; init; }
    public Dictionary<string, int> TicketsByBuilding { get; init; } = new();
}
