using Microsoft.EntityFrameworkCore;
using FixMyCampus.Domain.Entities;

namespace FixMyCampus.Application.Common.Interfaces;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<Ticket> Tickets { get; }
    DbSet<TicketStatusHistory> TicketStatusHistories { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
