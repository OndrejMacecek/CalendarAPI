using CalendarAPI.Domain.Common;
using System.Globalization;

namespace CalendarAPI.Domain.Calendars.Entities;

public sealed class Calendar : DomainBase
{
    public string Name { get; private set; }

    private Calendar(Guid id)
    {
        Id = id;
    }

    public static DomainResult<Calendar> Create(string name, Guid? id = null)
    {
        if (name == null) throw new ArgumentNullException(name);

        var calendar = new Calendar(id ?? Guid.NewGuid())
        {
            Name = name
        };

        return DomainResult<Calendar>.Success(calendar);
    }

    public static Calendar Rehydrate(Guid id, string name)
    {
        return new Calendar(id) { Name = name };
    }
}
