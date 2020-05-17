using DTO.Clinic;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Models.Clinic
{
    public class CreateClinicRequest : ClinicDto
    {
    }

    public class CreateClinicRequestValidator : AbstractValidator<CreateClinicRequest>
    {
        public CreateClinicRequestValidator()
        {
            RuleFor(x => x.Name)
             .MaximumLength(50)
             .NotEmpty();
        }
    }
}
