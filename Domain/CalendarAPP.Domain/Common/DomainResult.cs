namespace CalendarAPI.Domain.Common;
public class DomainResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public string? Error { get; }

    protected DomainResult(bool isSuccess, string? error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static DomainResult Success()
        => new(true, null);

    public static DomainResult Failure(string error)
        => new(false, error);
}

public sealed class DomainResult<T> : DomainResult
{
    public T? Value { get; }

    private DomainResult(bool isSuccess, T? value, string? error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static DomainResult<T> Success(T value)
        => new(true, value, null);

    public static new DomainResult<T> Failure(string error)
        => new(false, default, error);
}