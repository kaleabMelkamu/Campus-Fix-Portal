using Microsoft.EntityFrameworkCore;
using FixMyCampus.Application.Common.Exceptions;
using FixMyCampus.Application.Common.Interfaces;
using FixMyCampus.Application.DTOs;
using FixMyCampus.Domain.Entities;
using FixMyCampus.Domain.Enums;

namespace FixMyCampus.Application.Services;

public class TicketService : ITicketService
{
    private readonly IAppDbContext _db;

    public TicketService(IAppDbContext db)
    {
        _db = db;
    }

    public async Task<TicketDto> CreateTicketAsync(CreateTicketDto dto, Guid reporterId, CancellationToken ct = default)
    {
        var reporter = await _db.Users.FirstOrDefaultAsync(u => u.Id == reporterId, ct);
        if (reporter == null)
        {
            throw new NotFoundException($"Reporter user with ID '{reporterId}' not found.");
        }

        // Generate human-friendly ticket code e.g. TKT-20261005-4821
        var ticketNumber = $"TKT-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),
            TicketNumber = ticketNumber,
            Category = dto.Category,
            Building = dto.Building,
            Room = dto.Room.Trim(),
            Description = dto.Description.Trim(),
            Priority = dto.Priority,
            Status = TicketStatus.New,
            ReporterId = reporterId,
            CreatedAt = DateTime.UtcNow
        };

        // Seed initial history item
        ticket.StatusHistory.Add(new TicketStatusHistory
        {
            Id = Guid.NewGuid(),
            TicketId = ticket.Id,
            OldStatus = TicketStatus.New,
            NewStatus = TicketStatus.New,
            ChangedById = reporterId,
            ChangedAt = ticket.CreatedAt,
            Note = "Ticket submitted by reporter"
        });

        _db.Tickets.Add(ticket);
        await _db.SaveChangesAsync(ct);

        return MapToDto(ticket, reporter.FullName);
    }

    public async Task<PagedResponse<TicketDto>> GetCampusFeedAsync(TicketFilterDto filter, CancellationToken ct = default)
    {
        var query = _db.Tickets
            .AsNoTracking()
            .Include(t => t.Reporter)
            .AsQueryable();

        if (filter.Building.HasValue)
        {
            query = query.Where(t => t.Building == filter.Building.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(t => t.Status == filter.Status.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim().ToLower();
            query = query.Where(t =>
                t.Description.ToLower().Contains(search) ||
                t.Room.ToLower().Contains(search) ||
                t.TicketNumber.ToLower().Contains(search) ||
                (t.TechnicianName != null && t.TechnicianName.ToLower().Contains(search)));
        }

        var totalCount = await query.CountAsync(ct);

        var page = filter.Page < 1 ? 1 : filter.Page;
        var pageSize = filter.PageSize < 1 ? 20 : filter.PageSize;

        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TicketDto
            {
                Id = t.Id,
                TicketNumber = t.TicketNumber,
                Category = t.Category,
                Building = t.Building,
                Room = t.Room,
                Description = t.Description,
                Priority = t.Priority,
                Status = t.Status,
                ReporterId = t.ReporterId,
                ReporterName = t.Reporter.FullName,
                TechnicianId = t.TechnicianId,
                TechnicianName = t.TechnicianName,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                ResolvedAt = t.ResolvedAt
            })
            .ToListAsync(ct);

        return new PagedResponse<TicketDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<PagedResponse<TicketDto>> GetMyTicketsAsync(Guid reporterId, TicketFilterDto filter, CancellationToken ct = default)
    {
        var query = _db.Tickets
            .AsNoTracking()
            .Include(t => t.Reporter)
            .Where(t => t.ReporterId == reporterId)
            .AsQueryable();

        if (filter.Building.HasValue)
        {
            query = query.Where(t => t.Building == filter.Building.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(t => t.Status == filter.Status.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var search = filter.Search.Trim().ToLower();
            query = query.Where(t =>
                t.Description.ToLower().Contains(search) ||
                t.Room.ToLower().Contains(search) ||
                t.TicketNumber.ToLower().Contains(search));
        }

        var totalCount = await query.CountAsync(ct);
        var page = filter.Page < 1 ? 1 : filter.Page;
        var pageSize = filter.PageSize < 1 ? 20 : filter.PageSize;

        var items = await query
            .OrderByDescending(t => t.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(t => new TicketDto
            {
                Id = t.Id,
                TicketNumber = t.TicketNumber,
                Category = t.Category,
                Building = t.Building,
                Room = t.Room,
                Description = t.Description,
                Priority = t.Priority,
                Status = t.Status,
                ReporterId = t.ReporterId,
                ReporterName = t.Reporter.FullName,
                TechnicianId = t.TechnicianId,
                TechnicianName = t.TechnicianName,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt,
                ResolvedAt = t.ResolvedAt
            })
            .ToListAsync(ct);

        return new PagedResponse<TicketDto>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<TicketDetailDto> GetTicketByIdAsync(Guid id, CancellationToken ct = default)
    {
        var ticket = await _db.Tickets
            .AsNoTracking()
            .Include(t => t.Reporter)
            .Include(t => t.StatusHistory)
                .ThenInclude(h => h.ChangedBy)
            .FirstOrDefaultAsync(t => t.Id == id, ct);

        if (ticket == null)
        {
            throw new NotFoundException($"Ticket with ID '{id}' was not found.");
        }

        return new TicketDetailDto
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            Category = ticket.Category,
            Building = ticket.Building,
            Room = ticket.Room,
            Description = ticket.Description,
            Priority = ticket.Priority,
            Status = ticket.Status,
            ReporterId = ticket.ReporterId,
            ReporterName = ticket.Reporter?.FullName ?? string.Empty,
            TechnicianId = ticket.TechnicianId,
            TechnicianName = ticket.TechnicianName,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            ResolvedAt = ticket.ResolvedAt,
            StatusHistory = ticket.StatusHistory
                .OrderBy(h => h.ChangedAt)
                .Select(h => new TicketStatusHistoryDto
                {
                    Id = h.Id,
                    OldStatus = h.OldStatus,
                    NewStatus = h.NewStatus,
                    ChangedById = h.ChangedById,
                    ChangedByName = h.ChangedBy?.FullName ?? "System",
                    ChangedAt = h.ChangedAt,
                    Note = h.Note
                })
                .ToList()
        };
    }

    public async Task<TicketDto> AssignTechnicianAsync(Guid ticketId, AssignTicketDto dto, Guid adminId, CancellationToken ct = default)
    {
        var ticket = await _db.Tickets
            .Include(t => t.Reporter)
            .Include(t => t.StatusHistory)
            .FirstOrDefaultAsync(t => t.Id == ticketId, ct);

        if (ticket == null)
        {
            throw new NotFoundException($"Ticket with ID '{ticketId}' was not found.");
        }

        // Domain method enforces: Must be New -> Assigned, technician name required, adds history
        ticket.AssignTechnician(dto.TechnicianName, dto.TechnicianId, adminId, dto.Note);

        await _db.SaveChangesAsync(ct);

        return MapToDto(ticket, ticket.Reporter?.FullName ?? string.Empty);
    }

    public async Task<TicketDto> MoveStatusAsync(Guid ticketId, UpdateTicketStatusDto dto, Guid adminId, CancellationToken ct = default)
    {
        var ticket = await _db.Tickets
            .Include(t => t.Reporter)
            .Include(t => t.StatusHistory)
            .FirstOrDefaultAsync(t => t.Id == ticketId, ct);

        if (ticket == null)
        {
            throw new NotFoundException($"Ticket with ID '{ticketId}' was not found.");
        }

        // Domain method enforces: Strict path New -> Assigned -> InProgress -> Resolved, no skips, no backwards
        ticket.MoveStatus(dto.Status, adminId, dto.Note);

        await _db.SaveChangesAsync(ct);

        return MapToDto(ticket, ticket.Reporter?.FullName ?? string.Empty);
    }

    public async Task<AdminStatsDto> GetAdminStatsAsync(CancellationToken ct = default)
    {
        var total = await _db.Tickets.CountAsync(ct);
        var open = await _db.Tickets.CountAsync(t => t.Status != TicketStatus.Resolved, ct);
        var newCount = await _db.Tickets.CountAsync(t => t.Status == TicketStatus.New, ct);
        var assigned = await _db.Tickets.CountAsync(t => t.Status == TicketStatus.Assigned, ct);
        var inProgress = await _db.Tickets.CountAsync(t => t.Status == TicketStatus.InProgress, ct);
        var resolved = await _db.Tickets.CountAsync(t => t.Status == TicketStatus.Resolved, ct);

        var byBuildingList = await _db.Tickets
            .GroupBy(t => t.Building)
            .Select(g => new { Building = g.Key, Count = g.Count() })
            .ToListAsync(ct);

        var buildingStats = byBuildingList
            .ToDictionary(k => k.Building.ToString(), v => v.Count);

        return new AdminStatsDto
        {
            TotalTickets = total,
            OpenTickets = open,
            NewTickets = newCount,
            AssignedTickets = assigned,
            InProgressTickets = inProgress,
            ResolvedTickets = resolved,
            TicketsByBuilding = buildingStats
        };
    }

    private static TicketDto MapToDto(Ticket ticket, string reporterName)
    {
        return new TicketDto
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            Category = ticket.Category,
            Building = ticket.Building,
            Room = ticket.Room,
            Description = ticket.Description,
            Priority = ticket.Priority,
            Status = ticket.Status,
            ReporterId = ticket.ReporterId,
            ReporterName = reporterName,
            TechnicianId = ticket.TechnicianId,
            TechnicianName = ticket.TechnicianName,
            CreatedAt = ticket.CreatedAt,
            UpdatedAt = ticket.UpdatedAt,
            ResolvedAt = ticket.ResolvedAt
        };
    }
}
