using CalendarAPI.Application.Calendars.UnitOfWorks;
using CalendarAPI.Domain.Calendars.Repositories;
using CalendarAPI.Infrastructure.Calendars.Repositories;
using CalendarAPI.Infrastructure.Calendars.UnitOfWorks;
using CalendarAPI.Infrastructure.Common;
using CalendarAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CalendarAPI.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<ITimeProvider, SnapshotTimeProvider>();

        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")));


        services.AddScoped<ICalendarRepository, CalendarRepository>();
        services.AddScoped<ICalendarUnitOfWork, CalendarUnitOfWork>();

        return services;
    }
}