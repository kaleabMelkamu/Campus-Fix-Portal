using Microsoft.EntityFrameworkCore;
using FixMyCampus.Domain.Entities;

namespace FixMyCampus.Infrastructure.Data;

public class MyCampusDbContext : DbContext
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