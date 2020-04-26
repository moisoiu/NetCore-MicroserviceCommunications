﻿using AutoMapper;
using DTO.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace User.AutoMapperProfile
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<UserDto, GetUserResponse>()
                .ReverseMap();

            CreateMap<Entities.User, GetUserResponse>();

            CreateMap<UserDto, Entities.User>()
                .ReverseMap();

            CreateMap<Entities.User, Entities.User>();

            CreateMap<CreateUserCommand, Entities.User>();
        }
    }
}
