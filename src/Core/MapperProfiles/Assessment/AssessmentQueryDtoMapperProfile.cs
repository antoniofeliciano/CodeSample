using AutoMapper;
using Core.DTOs.Assessment;
using Core.Entities.Assessment;

namespace Core.MapperProfiles.Assessment
{
    public class AssessmentQueryDtoMapperProfile : Profile
    {
        public AssessmentQueryDtoMapperProfile()
        {
            CreateMap<AssessmentQuery, AssessmentQueryDto>().ReverseMap();
        }
    }
}
