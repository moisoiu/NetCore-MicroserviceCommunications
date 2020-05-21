using AutoMapper;
using DTO.Appointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Appointment.AutoMapperProfile
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CreateAppointmentCommand, Entities.Appointment>();

            CreateMap<Entities.Appointment, Entities.Appointment>();

            CreateMap<AppointmentDto, Entities.Appointment>()
                .ReverseMap();

            CreateMap<Entities.Appointment, GetAppointmentResponse>();

            CreateMap<AppointmentDto, GetAppointmentResponse>();
        }
    }
}
