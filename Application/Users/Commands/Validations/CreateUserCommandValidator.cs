using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarAPI.Application.Users.Commands.Validations;
public sealed class CreateUserCommandValidator
    : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Email is invalid.")
            .MaximumLength(255);

        RuleFor(x => x.displayName)
            .NotEmpty()
            .WithMessage("Display name is required.")
            .MaximumLength(150);
    }
}