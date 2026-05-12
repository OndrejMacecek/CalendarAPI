using CalendarApi.Contract;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Contracts.Responses.Users;

namespace CalendarAPI.Application.EventTypes.Queries;

public sealed record GetAvailableEventTypesQuery(Guid CalendarId)
    : IQuery<IReadOnlyCollection<EventTypeDto>>;