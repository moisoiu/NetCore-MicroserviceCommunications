using AutoMapper;
using Backoffice.Gateway.Models.Appointment.Requests;
using Backoffice.Gateway.Models.Clinic;
using Backoffice.Gateway.Models.Consultation;
using Backoffice.Gateway.Models.Patient;
using DTO.Appointment;
using DTO.Clinic;
using DTO.Consultation;
using DTO.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backoffice.Gateway.AutoMapperProfile
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CreatePatientRequest, CreatePatientCommand>();
            CreateMap<CreateClinicRequest, CreateClinicCommand>();
            CreateMap<CreateConsultationRequest, CreateConsultationCommand>();
            CreateMap<CreateAppointmentRequest, CreateAppointmentCommand>();
        }

    }
}
