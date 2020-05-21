using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DTO.Consultation
{
    public class ConsultationDto
    {
        public Guid ClinicId { get; set; }
        public string ClinicName { get; set; }
        public Guid PatientId { get; set; }
        public string PatientName { get; set; }
    }

    public class ConsultationDtoValidator : AbstractValidator<ConsultationDto>
    {
        public ConsultationDtoValidator()
        {
            RuleFor(x => x)
            .Must(x =>
                x.ClinicId != Guid.Empty ||
                !string.IsNullOrEmpty(x.ClinicName) ||
                x.PatientId != Guid.Empty ||
                !string.IsNullOrEmpty(x.PatientName)
            )
            .WithMessage("Must complete at least one field");
        }
    }
}
