using CalendarAPI.Application;
using CalendarAPI.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CalendarAPI.Boostrapper;

public static class DependencyInjection
{
    public static IServiceCollection AddCalendarApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddApplication();
        services.AddInfrastructure(configuration);

        return services;
    }
}