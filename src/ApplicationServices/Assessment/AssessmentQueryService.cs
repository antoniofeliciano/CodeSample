using ApplicationServices.Bases;
using AutoMapper;
using Core.DTOs.Assessment;
using Core.Entities.Assessment;
using Core.Helpers;
using Core.Interfaces.Repositories.Bases;
using Core.Interfaces.Services.Assessment;
using Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ApplicationServices.Assessment
{
    public class AssessmentQueryService : BaseTenantService, IAssessmentQueryService
    {
        private readonly ILogger<AssessmentQueryService> _logger;
        private readonly IBaseTenantEntityRepository<AssessmentQuery> _assessmentQueryRepository;
        private readonly IValidator<AssessmentQueryDto> _AssessmentQueryDtoValidator;
        private readonly IMapper _mapper;
        public AssessmentQueryService(
            ILogger<AssessmentQueryService> logger,
            IBaseTenantEntityRepository<AssessmentQuery> AssessmentQueryRepository,
            IValidator<AssessmentQueryDto> AssessmentQueryDtoValidator,
            IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
            _assessmentQueryRepository = AssessmentQueryRepository.SetTenantId(TenantId);
            _AssessmentQueryDtoValidator = AssessmentQueryDtoValidator;
            _mapper = mapper;
        }
        public async Task<Result> AddAssessmentQueryAsync(AssessmentQueryDto AssessmentQueryDto)
        {
            try
            {
                var validationResult = await _AssessmentQueryDtoValidator.ValidateAsync(AssessmentQueryDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                await _assessmentQueryRepository.Insert(_mapper.Map<AssessmentQuery>(AssessmentQueryDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail adding AssessmentQuery.");
                throw;
            }
        }
        public async Task<Result> EditAssessmentQueryAsync(AssessmentQueryDto AssessmentQueryDto)
        {
            try
            {
                var validationResult = await _AssessmentQueryDtoValidator.ValidateAsync(AssessmentQueryDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                if (!await _assessmentQueryRepository.ExistsNotRemovedAsync(s => s.Id == AssessmentQueryDto.Id)) return new ErrorResult("AssessmentQuery not found.");

                await _assessmentQueryRepository.Update(_mapper.Map<AssessmentQuery>(AssessmentQueryDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing AssessmentQuery.");
                throw;
            }
        }
        public async Task<Result> DeleteAssessmentQueryAsync(Guid AssessmentQueryId)
        {
            try
            {
                if (!await _assessmentQueryRepository.ExistsNotRemovedAsync(s => s.Id == AssessmentQueryId)) return new ErrorResult("AssessmentQuery not found.");
                await _assessmentQueryRepository.Delete(AssessmentQueryId);
                return new Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing AssessmentQuery.");
                throw;
            }
        }
        public async Task<Result<IEnumerable<AssessmentQueryDto>>> GetAssessmentQueryForGridAsync(string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction)
        {
            try
            {
                var query = _assessmentQueryRepository.GetTenantDbSet()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(r => r.Name.ToLower().Contains(searchTerm.ToLower()));
                }

                if (!string.IsNullOrEmpty(orderBy))
                {
                    query = query.OrderBy($"{orderBy} {(direction == "desc" ? "descending" : "ascending")}");
                }
                else
                {
                    query = query.OrderBy($"Name {(direction == "desc" ? "descending" : "ascending")}");
                }

                if (pageNumber.HasValue && pageSize.HasValue)
                {
                    query = query.Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
                }

                return new SuccessResult<IEnumerable<AssessmentQueryDto>>(_mapper.Map<IEnumerable<AssessmentQueryDto>>(await query.ToListAsync()));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail getting Assessment Queries.");
                throw;
            }
        }

        public async Task<Result<string>> GetAssessmentQueryFileAsync()
        {
            try
            {
                var queries = await _assessmentQueryRepository.GetDbSet().Where(a => a.IsActive).ToListAsync();
                if (!queries.Any()) return new NoContentResult<string>();
                var encripted = EncryptionUtils.EncryptString(JsonSerializer.Serialize(queries), EncryptionUtils.GenerateTempSecureKey());
                return new SuccessResult<string>(encripted);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail getting Assessment Queries.");
                throw;
            }
        }
    }
}
