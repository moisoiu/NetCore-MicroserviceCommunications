using DTO.Consultation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Models.Consultation
{
    public class UpdateConsultationRequest
    {
        public ConsultationDto Original { get; set; }
        public ConsultationDto Changed { get; set; }
    }

    public class UpdateConsultationRequestValidator : AbstractValidator<UpdateConsultationRequest>
    {
        public UpdateConsultationRequestValidator()
        {
            RuleFor(x => x.Changed)
                 .SetValidator(new ConsultationDtoValidator());

            RuleFor(x => x.Original)
                .SetValidator(new ConsultationDtoValidator());
        }
    }
}
