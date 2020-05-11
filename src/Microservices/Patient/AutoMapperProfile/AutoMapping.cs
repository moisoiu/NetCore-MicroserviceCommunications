using AutoMapper;
using DTO.Patient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patient.AutoMapperProfile
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<CreatePatientCommand, Entities.Patient>()
                .ForMember(x => x.DateOfBirth, x => x.MapFrom(x => x.DateOfBirth.Date))
                .ForMember(x => x.FirstName, x => x.MapFrom(x => x.FirstName.Trim()))
                .ForMember(x => x.LastName, x => x.MapFrom(x => x.LastName.Trim()));

            CreateMap<Entities.Patient, Entities.Patient>();

            CreateMap<PatientDto, Entities.Patient>()
                .ReverseMap();

            CreateMap<Entities.Patient, GetPatientResponse>();

            CreateMap<PatientDto, GetPatientResponse>();

        }
    }
}
