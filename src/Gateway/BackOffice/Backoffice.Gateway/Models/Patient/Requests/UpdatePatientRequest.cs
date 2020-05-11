using DTO.Patient;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Models.Patient
{
    public class UpdatePatientRequest
    {
        public PatientDto Original { get; set; }
        public PatientDto Changed { get; set; }
    }

    public class UpdatePatientRequestValidator : AbstractValidator<UpdatePatientRequest>
    {
        public UpdatePatientRequestValidator()
        {
            RuleFor(x => x.Changed)
                 .SetValidator(new PatientDtoValidator());

            RuleFor(x => x.Original)
                .SetValidator(new PatientDtoValidator());
        }
    }
}
