using DTO.Appointment;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Models.Appointment.Requests
{
    public class UpdateAppointmentRequest
    {
        public AppointmentDto Original { get; set; }
        public AppointmentDto Changed { get; set; }
    }

    public class UpdateAppointmentRequestValidator : AbstractValidator<UpdateAppointmentRequest>
    {
        public UpdateAppointmentRequestValidator()
        {
            RuleFor(x => x.Changed)
                 .SetValidator(new AppointmentDtoValidator());

            RuleFor(x => x.Original)
                .SetValidator(new AppointmentDtoValidator());
        }
    }
}
