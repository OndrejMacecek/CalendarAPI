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
            .Must(x => Enum.TryParse<EventTypeScope>(x, true, out _))
            .WithMessage("Invalid event type scope. (syste, user, calendar)");

        RuleFor(x => x.CalendarId)
            .NotEmpty()
            .When(x => x.Scope.Equals(EventTypeScope.Calendar.ToString(), StringComparison.OrdinalIgnoreCase))
            .WithMessage("Calendar id is required for calendar scope.");
    }
}