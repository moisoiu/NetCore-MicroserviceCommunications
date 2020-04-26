using DTO.Configuration;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backoffice.Gateway.Models.User
{
    public class AuthenticateUserRequest
    {
        public string Account { get; set; }
        public string Password { get; set; }
    }

    public class AuthenticateUserRequestValidator : AbstractValidator<AuthenticateUserRequest>
    {
        public AuthenticateUserRequestValidator()
        {
            RuleFor(x => x.Account)
                .NotEmpty();

            RuleFor(x => x.Password)
                .NotEmpty();
        }
    }
}
