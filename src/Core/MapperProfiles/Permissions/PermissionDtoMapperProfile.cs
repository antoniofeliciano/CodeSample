using AutoMapper;
using Core.DTOs.Permissions;
using Core.Entities.Permissions;

namespace Core.MapperProfiles.Permissions
{
    internal class PermissionDtoMapperProfile : Profile
    {
        public PermissionDtoMapperProfile()
        {
            CreateMap<Permission, PermissionDto>()
                .ForMember(dest => dest.AreaName, opt =>
                {
                    opt.MapFrom(src => src.Interface.Area!.Name);
                })
                .ForMember(dest => dest.RoleName, opt =>
                 {
                     opt.MapFrom(src => src.Role.Name);
                 })
                .ForMember(dest => dest.InterfaceName, opt =>
                 {
                     opt.MapFrom(src => src.Interface.Name);
                 });

            CreateMap<PermissionDto, Permission>();
        }
    }
}
