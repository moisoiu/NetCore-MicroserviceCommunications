using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.User
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Account { get; set; }
    }

    public class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
        {
            RuleFor(x => x)
               .Must(x =>
                   !string.IsNullOrEmpty(x.Account) ||
                   !string.IsNullOrEmpty(x.Email) ||
                   !string.IsNullOrEmpty(x.FirstName) ||
                   !string.IsNullOrEmpty(x.LastName)
               )
               .WithMessage("Must complete at least one field");

            RuleFor(x => x.Email)
                .EmailAddress();
        }
    }
}
