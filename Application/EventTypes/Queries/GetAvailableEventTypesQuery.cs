using CalendarApi.Contract;
using CalendarAPI.Application.Common.Messaging;

namespace CalendarAPI.Application.EventTypes.Queries;

public sealed record GetAvailableEventTypesQuery(Guid CalendarId)
    : IQuery<IReadOnlyCollection<EventTypeDto>>;