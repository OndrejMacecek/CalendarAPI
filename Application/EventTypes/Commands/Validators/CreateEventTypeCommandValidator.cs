using CalendarAPI.Domain.EventTypes.Aggregates;
using FluentValidation;

namespace CalendarAPI.Application.EventTypes.Commands.Validators;
public sealed class CreateEventTypeCommandValidator
    : AbstractValidator<CreateEventTypeCommand>
{
    public CreateEventTypeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Scope)
            .NotEmpty()
            .Must(BeValidScope)
            .WithMessage("Invalid event type scope.");

        RuleFor(x => x.CalendarId)
            .NotEmpty()
            .When(x => IsCalendarScope(x.Scope))
            .WithMessage("Calendar id is required for calendar scope.");
    }

    private static bool BeValidScope(string scope)
    {
        return Enum.TryParse<EventTypeScope>(scope, true, out _);
    }

    private static bool IsCalendarScope(string scope)
    {
        return scope.Equals(EventTypeScope.Calendar.ToString(), StringComparison.OrdinalIgnoreCase);
    }
}