using CalendarAPI.Domain.Common;
using CalendarAPI.Domain.Users.ValueObjects;

namespace CalendarAPI.Domain.Users.Aggregates;

public sealed class User
    : DomainAggregate
{
    public EmailAddress Email { get; private set; } = null!;
    public string DisplayName { get; private set; } = null!;

    private User(Guid id) 
        : base(id) { }

    public static DomainResult<User> Create(
        string email, 
        string displayName, 
        Guid? id = null)
    {
        var emailResult = EmailAddress.Create(email);

        if (emailResult.IsFailure)
        {
            return DomainResult<User>.Failure(emailResult.Error!);
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            return DomainResult<User>.Failure(
                new DomainError("user.display_name_required", "Display name is required."));
        }

        var user = new User(id ?? Guid.NewGuid())
        {
            DisplayName = displayName.Trim(),
            Email = emailResult.Value!,
        };

        return DomainResult<User>.Success(user);
    }

    public static User Rehydrate(Guid id, string email, string displayName)
    {
        var emailResult = EmailAddress.Create(email);

        return new User(id)
        {
            Email = emailResult.Value!,
            DisplayName = displayName,
        };
    }
}