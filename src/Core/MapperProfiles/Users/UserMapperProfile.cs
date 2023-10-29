using AutoMapper;
using Core.DTOs.Permissions;
using Core.Entities.Authentication;

namespace Core.MapperProfiles.Users
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<UserGridDto, User>().ReverseMap();
        }
    }
}
