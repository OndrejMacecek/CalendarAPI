using CalendarApi.Contract;
using CalendarAPI.Application.Common.Messaging;

namespace CalendarAPI.Application.CalendarEvents.Queries;
public sealed record GetMyInvitationsQuery
    : IQuery<IReadOnlyCollection<InvitationDto>>;