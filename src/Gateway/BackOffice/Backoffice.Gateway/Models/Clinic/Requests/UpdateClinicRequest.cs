using DTO.Clinic;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Models.Clinic
{
    public class UpdateClinicRequest
    {
        public ClinicDto Original { get; set; }
        public ClinicDto Changed { get; set; }
    }

    public class UpdateClinicRequestValidator : AbstractValidator<UpdateClinicRequest>
    {
        public UpdateClinicRequestValidator()
        {
            RuleFor(x => x.Changed)
              .SetValidator(new ClinicDtoValidator());

            RuleFor(x => x.Original)
                .SetValidator(new ClinicDtoValidator());
        }
    }
}
