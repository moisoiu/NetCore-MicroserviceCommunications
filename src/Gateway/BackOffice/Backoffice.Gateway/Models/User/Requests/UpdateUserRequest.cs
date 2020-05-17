using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.User
{
    public class UpdateUserRequest
    {
        public UserDto Original { get; set; }
        public UserDto Changed { get; set; }
    }

    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.Changed)
                .SetValidator(new UserDtoValidator());

            RuleFor(x => x.Original)
                .SetValidator(new UserDtoValidator());
        }
    }
}
