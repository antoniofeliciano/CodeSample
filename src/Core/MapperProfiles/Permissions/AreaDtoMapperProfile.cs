using AutoMapper;
using Core.DTOs.Permissions;
using Core.Entities.Permissions;

namespace Core.MapperProfiles.Permissions
{
    public class AreaDtoMapperProfile : Profile
    {
        public AreaDtoMapperProfile()
        {
            CreateMap<Area, AreaDto>().ReverseMap();
        }
    }
}
