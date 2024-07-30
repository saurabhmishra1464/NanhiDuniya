using AutoMapper;
using NanhiDuniya.Core.Models;
using NanhiDuniya.Core.Resources.AccountDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanhiDuniya.Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<RegistrationRequestDto, RegisterModel>();
            CreateMap<LoginRequestDto,LoginModel>();
            CreateMap<LoginResponse, LoginResponseResource>();
            CreateMap<LoginResponse, UserRefreshToken>()
    .ForSourceMember(login => login.Token, login => login.DoNotValidate());
           
        }
    }
}
