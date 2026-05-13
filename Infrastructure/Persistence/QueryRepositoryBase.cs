using CalendarAPI.Application.Common.Repository;

namespace CalendarAPI.Infrastructure.Persistence;

public abstract class QueryRepositoryBase
    :IQueryReposiotory
{
    protected readonly AppDbContext Context;

    protected QueryRepositoryBase(AppDbContext context)
    {
        Context = context;
    }
}
