using Backoffice.Gateway.Communications.Refit;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Gateway.Models.Appointment.Requests
{
    public class CreateAppointmentRequest
    {
        public Guid FromClinicId { get; set; }
        public Guid ToClinicId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CreateAppointmentRequestValidator : AbstractValidator<CreateAppointmentRequest>
    {
        public CreateAppointmentRequestValidator(
            IClinicApi clinicApi,
            IPatientApi patientApi)
        {

            RuleFor(x => x.FromClinicId)
                .NotEmpty()
                .WithMessage("Provide FromClinicId")
                .MustAsync(async (clinicId, cancellationToken) =>
                {
                    var clinic = await clinicApi.GetClinic(clinicId);
                    if (!clinic.IsSuccessStatusCode)
                        return false;

                    return true;
                })
                .WithMessage("No FromClinic found");

            RuleFor(x => x.ToClinicId)
                .NotEmpty()
                .WithMessage("Provide ToClinicId")
                .MustAsync(async (clinicId, cancellationToken) =>
                {
                    var clinic = await clinicApi.GetClinic(clinicId);
                    if (!clinic.IsSuccessStatusCode)
                        return false;

                    return true;
                })
                .WithMessage("No ToClinic found");

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
