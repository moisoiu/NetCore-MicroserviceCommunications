using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Appointment
{
    public class AppointmentDto
    {
        public Guid ToClinicId { get; set; }
        public string ToClinicName { get; set; }
        public Guid FromClinicId { get; set; }
        public string FromClinicName { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? IsActive { get; set; }
    }

    public class AppointmentDtoValidator : AbstractValidator<AppointmentDto>
    {
        public AppointmentDtoValidator()
        {
            RuleFor(x => x)
             .Must(x =>
                 x.ToClinicId != Guid.Empty ||
                 !string.IsNullOrEmpty(x.ToClinicName) ||
                 x.FromClinicId != Guid.Empty ||
                 !string.IsNullOrEmpty(x.FromClinicName) ||
                 x.PatientId != Guid.Empty ||
                 !string.IsNullOrEmpty(x.PatientName) ||
                 x.StartDate != default ||
                 x.EndDate != default
             )
             .WithMessage("Must complete at least one field");
        }
    }
}
