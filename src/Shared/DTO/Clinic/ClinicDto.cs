using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Clinic
{
    public class ClinicDto
    {
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }

    public class ClinicDtoValidator : AbstractValidator<ClinicDto>
    {
        public ClinicDtoValidator()
        {
            RuleFor(x => x)
            .Must(x =>
                !string.IsNullOrEmpty(x.Name)                
            )
            .WithMessage("Must complete at least one field");
        }
    }
}
