using AutoMapper;
using Core.DTOs.Assessment;
using Core.Entities.Assessment;
using System.Text;

namespace Core.MapperProfiles.Assessment
{
    public class AssessmentCollectDtoMapperProfile : Profile
    {
        public AssessmentCollectDtoMapperProfile()
        {
            CreateMap<AssessmentCollect, AssessmentCollectDto>();
            CreateMap<AssessmentCollectDto, AssessmentCollect>()
                .ForMember(dest => dest.CollectResult, opt =>
                {
                    opt.MapFrom(src => Encoding.UTF8.GetBytes(src.CollectResult));
                });
        }
    }
}
