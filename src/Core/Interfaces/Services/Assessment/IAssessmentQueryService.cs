using Core.DTOs.Assessment;
using Core.Results;

namespace Core.Interfaces.Services.Assessment
{
    public interface IAssessmentQueryService
    {
        Task<Result> AddAssessmentQueryAsync(AssessmentQueryDto contactDto);
        Task<Result> EditAssessmentQueryAsync(AssessmentQueryDto contactDto);
        Task<Result> DeleteAssessmentQueryAsync(Guid assessmentQuerytId);
        Task<Result<IEnumerable<AssessmentQueryDto>>> GetAssessmentQueryForGridAsync(string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction);
        Task<Result<string>> GetAssessmentQueryFileAsync();
    }
}
