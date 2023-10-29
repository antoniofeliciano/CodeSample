using AutoMapper;
using Core.DTOs.Permissions;
using Core.Entities.Permissions;

namespace Core.MapperProfiles.Permissions
{
    public class InterfaceDtoMapperProfile : Profile
    {
        public InterfaceDtoMapperProfile()
        {
            CreateMap<Interface, InterfaceDto>().ReverseMap();
            CreateMap<Interface, InterfaceGridDto>()
                .ForMember(dest => dest.Area, opt =>
            {
                opt.MapFrom(src => src.Area!.Name);
            });
        }
    }
}
