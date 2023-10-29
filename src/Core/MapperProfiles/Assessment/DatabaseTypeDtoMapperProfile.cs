using AutoMapper;
using Core.DTOs.Assessment;
using Core.Entities.Assessment;

namespace Core.MapperProfiles.Assessment
{
    public class DatabaseTypeDtoMapperProfile : Profile
    {
        public DatabaseTypeDtoMapperProfile()
        {
            CreateMap<DatabaseType, DatabaseTypeDto>().ReverseMap();
        }
    }
}
