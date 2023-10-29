using AutoMapper;
using Core.DTOs.Authentication;
using Core.Entities.Authentication;

namespace Core.MapperProfiles.Authentication
{
    internal class SystemApiKeyDtoMapperProfile : Profile
    {
        public SystemApiKeyDtoMapperProfile()
        {
            CreateMap<SystemApiKey, SystemApiKeyDto>().ReverseMap();
        }
    }
}
