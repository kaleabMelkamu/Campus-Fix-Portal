using Microsoft.EntityFrameworkCore;
using FixMyCampus.Application.Common.Interfaces;
using FixMyCampus.Domain.Entities;

namespace FixMyCampus.Infrastructure.Data;

public class MyCampusDbContext : DbContext, IAppDbContext
{
    public MyCampusDbContext(
        DbContextOptions<MyCampusDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();

    public DbSet<Ticket> Tickets => Set<Ticket>();

    public DbSet<TicketStatusHistory> TicketStatusHistories =>
        Set<TicketStatusHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(MyCampusDbContext).Assembly);
    }
}