using CalendarAPI.Application.Common.UnitOfWork;
using CalendarAPI.Domain.CalendarEvents.Repositories;
using CalendarAPI.Domain.Calendars.Repositories;
using CalendarAPI.Domain.EventTypes.Repositories;
using CalendarAPI.Domain.Users.Repositories;
using CalendarAPI.Infrastructure.CalendarEvents.Repositories;
using CalendarAPI.Infrastructure.Calendars.Repositories;
using CalendarAPI.Infrastructure.Common;
using CalendarAPI.Infrastructure.EventTypes.Repositories;
using CalendarAPI.Infrastructure.Persistence;
using CalendarAPI.Infrastructure.Users.Repositories;
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


        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICalendarRepository, CalendarRepository>();
        services.AddScoped<IEventTypeRepository, EventTypeRepository>();
        services.AddScoped<ICalendarEventRepository, CalendarEventRepository>();

        services.AddScoped<ICalendarApiUnitOfWork, CalendarApiUnitOfWork>();

        return services;
    }
}