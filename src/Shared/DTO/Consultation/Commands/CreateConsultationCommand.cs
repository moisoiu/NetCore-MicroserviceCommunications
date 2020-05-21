using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DTO.Consultation
{
    public class CreateConsultationCommand : ConsultationDto
    {
        public Guid CreatedBy { get; set; }
    }

    public class CreateConsultationCommandValidator : AbstractValidator<CreateConsultationCommand>
    {
        public CreateConsultationCommandValidator()
        {
            RuleFor(x => x.ClinicId)
                .NotEmpty();

            RuleFor(x => x.ClinicName)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.CreatedBy)
                .NotEmpty();

            RuleFor(x => x.PatientId)
                .NotEmpty();

            RuleFor(x => x.PatientName)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
