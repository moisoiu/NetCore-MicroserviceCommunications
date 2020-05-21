using AutoMapper;
using DTO.Consultation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Consultation.AutoMapperProfile
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CreateConsultationCommand, Entities.Consultation>();

            CreateMap<Entities.Consultation, Entities.Consultation>();

            CreateMap<ConsultationDto, Entities.Consultation>()
                .ReverseMap();

            CreateMap<Entities.Consultation, GetConsultationResponse>();

            CreateMap<ConsultationDto, GetConsultationResponse>();
        }
    }
}
