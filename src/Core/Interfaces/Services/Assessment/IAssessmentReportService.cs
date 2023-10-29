using Core.DTOs.Assessment;
using Core.Results;

namespace Core.Interfaces.Services.Assessment
{
    public interface IAssessmentCollectService
    {
        Task<Result> AddAssessmentCollectAsync(AssessmentCollectDto contactDto);
        Task<Result> AddEncriptedCollectAsync(EncriptedCollectDto encriptedCollect);
        Task<Result> EditAssessmentCollectAsync(AssessmentCollectDto contactDto);
        Task<Result> DeleteAssessmentCollectAsync(Guid assessmentCollecttId);
        Task<Result<IEnumerable<AssessmentCollectGridDto>>> GetAssessmentCollectForGridAsync(string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction);
    }
}
