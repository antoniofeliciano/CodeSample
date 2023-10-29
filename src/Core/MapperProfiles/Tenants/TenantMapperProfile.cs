using AutoMapper;
using Core.DTOs.Tenants;
using Core.Entities.Tenants;

namespace Core.MapperProfiles.Tenants
{
    public class TenantMapperProfile : Profile
    {
        public TenantMapperProfile()
        {
            CreateMap<Tenant, TenantDto>().ReverseMap();
            CreateMap<Tenant, TenantGridDto>().ReverseMap();
        }
    }
}
