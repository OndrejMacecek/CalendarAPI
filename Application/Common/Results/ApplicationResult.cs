using CalendarAPI.Domain.Common;

namespace CalendarAPI.Application.Common.Results;

public sealed record Error(string Code, string Message);

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    protected Result(bool isSuccess, Error? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
        => new(true, null);

    public static Result Failure(Error error)
        => new(false, error);
}

public sealed class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, Error? error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value)
        => new(true, value, null);

    public static new Result<T> Failure(Error error)
        => new(false, default, error);
}

public static class ResultExtensions
{
    public static Result<T> ToApplicationFailure<T>(this DomainError error)
    {
        return Result<T>.Failure(new Error(error.Code, error.Message));
    }

    public static Result ToApplicationFailure(this DomainError error)
    {
        return Result.Failure(new Error(error.Code, error.Message));
    }
}