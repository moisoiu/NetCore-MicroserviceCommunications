using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.User
{
    public class CreateUserCommand : UserDto
    {
        public string Password { get; set; }
    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.Account)
                 .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.Password)
                .MinimumLength(10);

            RuleFor(x => x.FirstName)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotEmpty();

            RuleFor(x => x.Email)
                .EmailAddress();
        }
    }
}
