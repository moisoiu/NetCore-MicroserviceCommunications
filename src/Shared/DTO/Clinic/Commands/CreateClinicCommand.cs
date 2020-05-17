using DTO.User;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Clinic
{
    public class CreateClinicCommand : ClinicDto
    {
        public Guid CreatedBy { get; set; }
    }

    public class CreateClinicCommandValidator : AbstractValidator<CreateClinicCommand>
    {
        public CreateClinicCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.CreatedBy)
                .NotEmpty();
        }
    }
}
