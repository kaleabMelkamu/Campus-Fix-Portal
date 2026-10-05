using Microsoft.EntityFrameworkCore;
using FixMyCampus.Application.Common.Security;
using FixMyCampus.Domain.Entities;
using FixMyCampus.Domain.Enums;

namespace FixMyCampus.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(MyCampusDbContext db)
    {
        // Ensure database exists/migrated
        await db.Database.MigrateAsync();

        if (await db.Users.AnyAsync())
        {
            return; // Already seeded
        }

        // 1. Seed Required Test Accounts
        var adminUser = new User
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            FullName = "Campus Admin",
            Email = "admin@hackathon.local",
            PasswordHash = PasswordHasher.Hash("Admin123!"),
            Role = UserRole.Admin,
            CreatedAt = DateTime.UtcNow
        };

        var reporterUser = new User
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            FullName = "Student Reporter",
            Email = "user@hackathon.local",
            PasswordHash = PasswordHasher.Hash("User123!"),
            Role = UserRole.Reporter,
            CreatedAt = DateTime.UtcNow
        };

        var techUser = new User
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            FullName = "Dawit Technician",
            Email = "tech@hackathon.local",
            PasswordHash = PasswordHasher.Hash("Tech123!"),
            Role = UserRole.Technician,
            CreatedAt = DateTime.UtcNow
        };

        db.Users.AddRange(adminUser, reporterUser, techUser);
        await db.SaveChangesAsync();

        // 2. Seed Sample Tickets Across Lifecycle States for Live Demo

        // Ticket 1: New
        var ticketNew = new Ticket
        {
            Id = Guid.NewGuid(),
            TicketNumber = "TKT-20261005-1001",
            Category = TicketCategory.Electricity,
            Building = Building.Library,
            Room = "Floor 2 Study Hall",
            Description = "Flickering overhead lights and one dead outlet near the west windows.",
            Priority = ComplaintPriority.High,
            Status = TicketStatus.New,
            ReporterId = reporterUser.Id,
            CreatedAt = DateTime.UtcNow.AddHours(-4)
        };
        ticketNew.StatusHistory.Add(new TicketStatusHistory
        {
            Id = Guid.NewGuid(),
            TicketId = ticketNew.Id,
            OldStatus = TicketStatus.New,
            NewStatus = TicketStatus.New,
            ChangedById = reporterUser.Id,
            ChangedAt = ticketNew.CreatedAt,
            Note = "Reported via mobile portal"
        });

        // Ticket 2: Assigned
        var ticketAssigned = new Ticket
        {
            Id = Guid.NewGuid(),
            TicketNumber = "TKT-20261005-1002",
            Category = TicketCategory.Plumbing,
            Building = Building.StudentCenter,
            Room = "Restroom B1",
            Description = "Sink faucet leaking water continuously onto the floor.",
            Priority = ComplaintPriority.Medium,
            Status = TicketStatus.Assigned,
            ReporterId = reporterUser.Id,
            TechnicianId = techUser.Id,
            TechnicianName = "Dawit Technician",
            CreatedAt = DateTime.UtcNow.AddHours(-6),
            UpdatedAt = DateTime.UtcNow.AddHours(-3)
        };
        ticketAssigned.StatusHistory.Add(new TicketStatusHistory
        {
            Id = Guid.NewGuid(),
            TicketId = ticketAssigned.Id,
            OldStatus = TicketStatus.New,
            NewStatus = TicketStatus.New,
            ChangedById = reporterUser.Id,
            ChangedAt = ticketAssigned.CreatedAt,
            Note = "Initial report"
        });
        ticketAssigned.StatusHistory.Add(new TicketStatusHistory
        {
            Id = Guid.NewGuid(),
            TicketId = ticketAssigned.Id,
            OldStatus = TicketStatus.New,
            NewStatus = TicketStatus.Assigned,
            ChangedById = adminUser.Id,
            ChangedAt = ticketAssigned.UpdatedAt!.Value,
            Note = "Assigned to Dawit Technician"
        });

        // Ticket 3: In Progress
        var ticketInProgress = new Ticket
        {
            Id = Guid.NewGuid(),
            TicketNumber = "TKT-20261005-1003",
            Category = TicketCategory.Network,
            Building = Building.ComputerLab,
            Room = "Lab 304",
            Description = "Wi-Fi access point AP-304 dropping packet connections every 2 minutes.",
            Priority = ComplaintPriority.Critical,
            Status = TicketStatus.InProgress,
            ReporterId = reporterUser.Id,
            TechnicianId = techUser.Id,
            TechnicianName = "Dawit Technician",
            CreatedAt = DateTime.UtcNow.AddHours(-8),
            UpdatedAt = DateTime.UtcNow.AddHours(-2)
        };
        ticketInProgress.StatusHistory.Add(new TicketStatusHistory
        {
            Id = Guid.NewGuid(),
            TicketId = ticketInProgress.Id,
            OldStatus = TicketStatus.New,
            NewStatus = TicketStatus.New,
            ChangedById = reporterUser.Id,
            ChangedAt = ticketInProgress.CreatedAt,
            Note = "Ticket submitted"
        });
        ticketInProgress.StatusHistory.Add(new TicketStatusHistory
        {
            Id = Guid.NewGuid(),
            TicketId = ticketInProgress.Id,
            OldStatus = TicketStatus.New,
            NewStatus = TicketStatus.Assigned,
            ChangedById = adminUser.Id,
            ChangedAt = ticketInProgress.CreatedAt.AddHours(2),
            Note = "Assigned to Dawit Technician"
        });
        ticketInProgress.StatusHistory.Add(new TicketStatusHistory
        {
            Id = Guid.NewGuid(),
            TicketId = ticketInProgress.Id,
            OldStatus = TicketStatus.Assigned,
            NewStatus = TicketStatus.InProgress,
            ChangedById = adminUser.Id,
            ChangedAt = ticketInProgress.UpdatedAt!.Value,
            Note = "Technician arrived on site, diagnostics underway"
        });

        // Ticket 4: Resolved
        var ticketResolved = new Ticket
        {
            Id = Guid.NewGuid(),
            TicketNumber = "TKT-20261005-1004",
            Category = TicketCategory.Projector,
            Building = Building.ScienceBuilding,
            Room = "Lecture Hall B",
            Description = "HDMI cable replaced and projector lamp recalibrated.",
            Priority = ComplaintPriority.Low,
            Status = TicketStatus.Resolved,
            ReporterId = reporterUser.Id,
            TechnicianId = techUser.Id,
            TechnicianName = "Dawit Technician",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow.AddHours(-5),
            ResolvedAt = DateTime.UtcNow.AddHours(-5)
        };
        ticketResolved.StatusHistory.Add(new TicketStatusHistory
        {
            Id = Guid.NewGuid(),
            TicketId = ticketResolved.Id,
            OldStatus = TicketStatus.New,
            NewStatus = TicketStatus.New,
            ChangedById = reporterUser.Id,
            ChangedAt = ticketResolved.CreatedAt,
            Note = "Ticket submitted"
        });
        ticketResolved.StatusHistory.Add(new TicketStatusHistory
        {
            Id = Guid.NewGuid(),
            TicketId = ticketResolved.Id,
            OldStatus = TicketStatus.New,
            NewStatus = TicketStatus.Assigned,
            ChangedById = adminUser.Id,
            ChangedAt = ticketResolved.CreatedAt.AddHours(1),
            Note = "Assigned to Dawit Technician"
        });
        ticketResolved.StatusHistory.Add(new TicketStatusHistory
        {
            Id = Guid.NewGuid(),
            TicketId = ticketResolved.Id,
            OldStatus = TicketStatus.Assigned,
            NewStatus = TicketStatus.InProgress,
            ChangedById = adminUser.Id,
            ChangedAt = ticketResolved.CreatedAt.AddHours(2),
            Note = "Replacement lamp requested from storage"
        });
        ticketResolved.StatusHistory.Add(new TicketStatusHistory
        {
            Id = Guid.NewGuid(),
            TicketId = ticketResolved.Id,
            OldStatus = TicketStatus.InProgress,
            NewStatus = TicketStatus.Resolved,
            ChangedById = adminUser.Id,
            ChangedAt = ticketResolved.ResolvedAt!.Value,
            Note = "Lamp installed and display tested successfully"
        });

        db.Tickets.AddRange(ticketNew, ticketAssigned, ticketInProgress, ticketResolved);
        await db.SaveChangesAsync();
    }
}
