using FluentValidation;

namespace CalendarAPI.Application.Commands.Validators;
public sealed class CreateCalendarCommandValidator
    : AbstractValidator<CreateCalendarCommand>
{
    public CreateCalendarCommandValidator()
    {
        RuleFor(x => x.Calendar.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
