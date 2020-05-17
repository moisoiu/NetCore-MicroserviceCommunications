using AutoMapper;
using DTO.Clinic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Clinic.AutoMapperProfile
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CreateClinicCommand, Entities.Clinic>();

            CreateMap<Entities.Clinic, Entities.Clinic>();

            CreateMap<ClinicDto, Entities.Clinic>()
                .ReverseMap();

            CreateMap<Entities.Clinic, GetClinicResponse>();

            CreateMap<ClinicDto, GetClinicResponse>();
        }
    }
}
