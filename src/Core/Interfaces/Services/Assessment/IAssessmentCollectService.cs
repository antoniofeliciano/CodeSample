using Core.DTOs.Assessment;
using Core.Results;

namespace Core.Interfaces.Services.Assessment
{
    public interface IAssessmentReportService
    {
        Task<Result<object>> GetAssessmentCollectDataAsync(Guid collectId);
    }
}
