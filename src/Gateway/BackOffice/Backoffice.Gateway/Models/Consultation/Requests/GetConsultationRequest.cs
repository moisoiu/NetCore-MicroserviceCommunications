using DTO.Consultation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Models.Consultation
{
    public class GetConsultationRequest : ConsultationDto
    {
    }

    public class GetConsultationsRequestValidator : AbstractValidator<GetConsultationRequest>
    {
        public GetConsultationsRequestValidator()
        {
            RuleFor(x => x.ClinicId)
                 .NotEmpty()
                 .When(x => string.IsNullOrEmpty(x.ClinicName));

            RuleFor(x => x.ClinicName)
                 .NotEmpty()
                 .When(x => x.ClinicId == Guid.Empty);

            RuleFor(x=>x.PatientId)
                .NotEmpty()
                .When(x => string.IsNullOrEmpty(x.PatientName));

            RuleFor(x => x.PatientName)
                 .NotEmpty()
                 .When(x => x.PatientId == Guid.Empty);
        }
    }
}
