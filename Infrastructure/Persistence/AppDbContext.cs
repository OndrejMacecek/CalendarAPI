using CalendarAPI.Infrastructure.Calendars.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalendarAPI.Infrastructure.Persistence;
public sealed class AppDbContext 
    : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<CalendarEntity> Calendars => Set<CalendarEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(AppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}