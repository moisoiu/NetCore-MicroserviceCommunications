using Backoffice.Gateway.Communications.Refit;
using DTO.Consultation;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Models.Consultation
{
    public class CreateConsultationRequest
    {
        public Guid ClinicId { get; set; }
        public Guid PatientId { get; set; }
    }

    public class CreateConsultationRequestValidator : AbstractValidator<CreateConsultationRequest>
    {
        public CreateConsultationRequestValidator(
            IClinicApi clinicApi,
            IPatientApi patientApi)
        {


            RuleFor(x => x.ClinicId)
                .NotEmpty()
                .WithMessage("Provide ClinicId")
                .MustAsync(async (clinicId, cancellationToken) =>
                {
                    var clinic = await clinicApi.GetClinic(clinicId);
                    if (!clinic.IsSuccessStatusCode)
                        return false;

                    return true;
                })
                .WithMessage("Clinic does not exists");

            RuleFor(x => x.PatientId)
                .NotEmpty()
                .WithMessage("Provide PatientId")
                .MustAsync(async (patientId, cancellationToken) =>
                {
                    var patient = await patientApi.GetPatient(patientId);
                    if (!patient.IsSuccessStatusCode)
                        return false;

                    return true;
                })
                .WithMessage("Patient does not exists");

        }
    }
}
