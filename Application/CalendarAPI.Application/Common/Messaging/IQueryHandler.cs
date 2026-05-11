using CalendarAPI.Application.Common.Results;
using MediatR;

namespace CalendarAPI.Application.Common.Messaging;
public interface IQueryHandler<TQuery, TResponse>
    : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}