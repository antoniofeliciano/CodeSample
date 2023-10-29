using ApplicationServices.Bases;
using Core.Entities.Assessment;
using Core.Interfaces.Repositories.Bases;
using Core.Interfaces.Services.Assessment;
using Core.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.Text.Encodings.Web;
using System.Text;
using System.Collections;
using Core.Helpers;
using System.Text.Json;

namespace ApplicationServices.Assessment
{
    public class AssessmentReportService : BaseTenantService, IAssessmentReportService
    {
        private readonly ILogger<AssessmentReportService> _logger;
        private readonly IBaseTenantEntityRepository<AssessmentCollect> _assessmentCollectRepository;

        public AssessmentReportService(
            ILogger<AssessmentReportService> logger,
            IBaseTenantEntityRepository<AssessmentCollect> assessmentCollectRepository,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
            _assessmentCollectRepository = assessmentCollectRepository.SetTenantId(TenantId);

        }

        public async Task<Result<object>> GetAssessmentCollectDataAsync(Guid collectId)
        {
            try
            {
                var result = await _assessmentCollectRepository.GetTenantDbSet().Select(c => new { c.CollectResult ,c.Id}).FirstAsync(c=>c.Id == collectId);

                var encriptedStr = Encoding.UTF8.GetString(result.CollectResult);
                var jsonResult = EncryptionUtils.DecryptString(encriptedStr, EncryptionUtils.GenerateTempSecureKey());

                return new SuccessResult<object>(jsonResult);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail getting AssessmentCollects.");
                throw;
            }
        }
    }
}
