using FluentValidation;

namespace CalendarAPI.Application.CalendarEvents.Commands.Validators;
public sealed class CreateCalendarEventCommandValidator
    : AbstractValidator<CreateCalendarEventCommand>
{
    public CreateCalendarEventCommandValidator()
    {
        RuleFor(x => x.CalendarId).NotEmpty();
        RuleFor(x => x.EventTypeId).NotEmpty();

        RuleFor(x => x.EndAtUtc)
            .GreaterThan(x => x.StartAtUtc);

    }
}