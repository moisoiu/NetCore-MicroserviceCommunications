using DTO.Appointment;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Models.Appointment.Requests
{
    public class GetAppointmentRequest : AppointmentDto
    {
    }
    
    public class GetAppointmentRequestValidator : AbstractValidator<GetAppointmentRequest>
    {
        public GetAppointmentRequestValidator()
        {
            RuleFor(x => x.FromClinicId)
                .NotEmpty()
                .When(x => string.IsNullOrEmpty(x.FromClinicName));

            RuleFor(x => x.FromClinicName)
                 .NotEmpty()
                 .When(x => x.FromClinicId == Guid.Empty);

            RuleFor(x => x.ToClinicId)
                .NotEmpty()
                .When(x => string.IsNullOrEmpty(x.ToClinicName));

            RuleFor(x => x.ToClinicName)
                 .NotEmpty()
                 .When(x => x.ToClinicId == Guid.Empty);

            RuleFor(x => x.PatientId)
                .NotEmpty()
                .When(x => string.IsNullOrEmpty(x.PatientName));

            RuleFor(x => x.PatientName)
                 .NotEmpty()
                 .When(x => x.PatientId == Guid.Empty);
        }
    }
}
