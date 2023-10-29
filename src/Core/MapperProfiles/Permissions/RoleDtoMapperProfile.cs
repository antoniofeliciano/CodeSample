using AutoMapper;
using Core.DTOs.Authentication;
using Core.Entities.Authentication;

namespace Core.MapperProfiles.Permissions
{
    public class RoleDtoMapperProfile : Profile
    {
        public RoleDtoMapperProfile()
        {
            CreateMap<Role, RoleDto>().ReverseMap();
        }
    }
}
