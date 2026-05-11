using CalendarAPI.Domain.Common;

namespace CalendarAPI.Domain.Users.ValueObjects;
public sealed class EmailAddress 
    : ValueObject
{
    public string Value { get; }

    private EmailAddress(string value)
    {
        Value = value;
    }

    public static DomainResult<EmailAddress> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return DomainResult<EmailAddress>.Failure(
                new DomainError("user.email_required", "Email is required."));

        value = value.Trim().ToLowerInvariant();

        if (!value.Contains('@'))
            return DomainResult<EmailAddress>.Failure(
                new DomainError("user.email_invalid", "Email is invalid."));

        return DomainResult<EmailAddress>.Success(new EmailAddress(value));
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}