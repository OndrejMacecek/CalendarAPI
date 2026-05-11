using CalendarAPI.Application.Common.Results;
using MediatR;

namespace CalendarAPI.Application.Common.Messaging;
public interface IQuery<TResponse>
    : IRequest<Result<TResponse>>
{
}
