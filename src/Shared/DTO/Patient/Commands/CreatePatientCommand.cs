using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Patient
{
    public class CreatePatientCommand : PatientDto
    {
        public Guid CreatedBy { get; set; }
    }

    public class CreatePatientCommandValidator : AbstractValidator<CreatePatientCommand>
    {
        public CreatePatientCommandValidator()
        {
            RuleFor(x => x.CreatedBy)
                .NotEmpty();

            RuleFor(x => x.DateOfBirth)
                .NotEmpty();

            RuleFor(x => x.FirstName)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .NotEmpty();
        }
    }
}
