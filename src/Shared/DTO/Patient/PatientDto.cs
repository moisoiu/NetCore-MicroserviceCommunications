using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Patient
{
    public class PatientDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class PatientDtoValidator : AbstractValidator<PatientDto>
    {
        public PatientDtoValidator()
        {
            RuleFor(x => x)
             .Must(x =>
                 x.DateOfBirth != default ||
                 !string.IsNullOrEmpty(x.FirstName) ||
                 !string.IsNullOrEmpty(x.LastName)
             )
             .WithMessage("Must complete at least one field");
        }
    }
}
