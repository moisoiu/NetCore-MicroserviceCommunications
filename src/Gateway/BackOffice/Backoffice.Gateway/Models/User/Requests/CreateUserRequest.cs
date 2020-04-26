using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.User
{
    public class CreateUserRequest : UserDto
    {
        public string Password { get; set; }
    }

    public class CreateUserRequestValdiator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValdiator()
        {
            RuleFor(x => x.Account)
                 .NotEmpty();

            RuleFor(x => x.Account)
                .MinimumLength(6)
                .MaximumLength(6)
                .WithMessage("Minimum and Maximum Length for Account is 6");

            RuleFor(x => x.Account)
                .Matches(@"[A-Z]{2}\d{4}")
                .WithMessage("Account must be in format 2 letters and 4 numbers. Ex: aa0000");

            RuleFor(x => x.Password)
                .NotEmpty();

            RuleFor(x => x.Password)
                .MinimumLength(10);

            RuleFor(x => x.FirstName)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotEmpty();
        }
    }
}
