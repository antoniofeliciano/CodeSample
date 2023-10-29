using AutoMapper;
using Core.DTOs.Authentication;
using Core.Entities.Authentication;

namespace Core.MapperProfiles.Authentication
{
    internal class RoleDtoMapperProfile : Profile
    {
        public RoleDtoMapperProfile()
        {
            CreateMap<Role, RoleDto>().ReverseMap();
        }
    }
}
