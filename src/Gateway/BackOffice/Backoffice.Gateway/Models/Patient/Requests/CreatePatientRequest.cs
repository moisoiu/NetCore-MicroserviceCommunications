using DTO.Patient;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Models.Patient
{
    public class CreatePatientRequest : PatientDto
    {
    }

    public class CreatePatientRequestValidator : AbstractValidator<CreatePatientCommand>
    {
        public CreatePatientRequestValidator()
        {
            RuleFor(x => x.CreatedBy)
               .NotEmpty();

            RuleFor(x => x.DateOfBirth)
                .NotEmpty();

            RuleFor(x => x.DateOfBirth)
                .Must(x => x.Year < DateTime.Today.Year);

            RuleFor(x => x.FirstName)                
                .MaximumLength(50)
                .NotEmpty();

            RuleFor(x => x.LastName)
                .MaximumLength(50)
                .NotEmpty();
        }
    }
}
