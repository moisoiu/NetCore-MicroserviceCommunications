using AutoMapper;
using Backoffice.Gateway.Models.Patient;
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
        }

    }
}
