using CalendarAPI.Application.Common.Results;
using MediatR;

namespace CalendarAPI.Application.Common.Messaging;
public interface ICommand 
    : IRequest<Result>
{
}

public interface ICommand<TResponse> 
    : IRequest<Result<TResponse>>
{
}