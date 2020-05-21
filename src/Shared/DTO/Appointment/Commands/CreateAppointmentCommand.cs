using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Appointment
{
    public class CreateAppointmentCommand : AppointmentDto
    {
        public Guid CreatedBy { get; set; }
    }

    public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
    {
        public CreateAppointmentCommandValidator()
        {
            RuleFor(x => x.ToClinicId)
               .NotEmpty();

            RuleFor(x => x.ToClinicName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.CreatedBy)
                .NotEmpty();

            RuleFor(x => x.PatientName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.PatientId)
                .NotEmpty();

            RuleFor(x => x.StartDate)
                .NotEmpty();

            RuleFor(x => x.EndDate)
                .NotEmpty();

            RuleFor(x => x.FromClinicId)
                .NotEmpty();

            RuleFor(x => x.FromClinicName)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
