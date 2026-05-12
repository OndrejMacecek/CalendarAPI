using CalendarApi.Contract;
using CalendarAPI.Application.Common.Interfaces;
using CalendarAPI.Application.Common.Messaging;
using CalendarAPI.Application.Common.Results;
using CalendarAPI.Application.Common.UnitOfWork;

namespace CalendarAPI.Application.CalendarEvents.Queries.Handlers;
public sealed class GetMyInvitationsQueryHandler
    : IQueryHandler<GetMyInvitationsQuery, IReadOnlyCollection<InvitationDto>>
{
    private readonly ICalendarApiUnitOfWork _uow;
    private readonly ICurrentUser _currentUser;

    public GetMyInvitationsQueryHandler(
        ICalendarApiUnitOfWork uow,
        ICurrentUser currentUser)
    {
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Result<IReadOnlyCollection<InvitationDto>>> Handle(
        GetMyInvitationsQuery request,
        CancellationToken cancellationToken)
    {
        var userId = _currentUser.UserId;

        if (userId == Guid.Empty)
        {
            return Result<IReadOnlyCollection<InvitationDto>>.Failure(
                new Error("auth.user_missing", "Missing or invalid user header."));
        }

        var events = await _uow.CalendarEvents.GetInvitationsForUserAsync(
            userId,
            cancellationToken);

        var result = events
            .Select(e =>
            {
                var participant = e.Participants.First(p => p.UserId == userId);

                return new InvitationDto()
                {
                    EventId = e.Id,
                    CalendarId = e.CalendarId,
                    OwnerUserId = e.OwnerUserId,
                    EventTypeId = e.EventTypeId,
                    Title = e.Title,
                    Description = e.Description,
                    StartAtUtc = e.TimeRange.StartAtUtc,
                    EndAtUtc = e.TimeRange.EndAtUtc,
                    Statu = participant.Status.ToString()
                };
            })
            .ToList();

        return Result<IReadOnlyCollection<InvitationDto>>.Success(result);
    }
}