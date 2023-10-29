using ApplicationServices.Bases;
using AutoMapper;
using Core.DTOs.Assessment;
using Core.Entities.Assessment;
using Core.Exceptions;
using Core.Helpers;
using Core.Interfaces.Repositories.Bases;
using Core.Interfaces.Services.Assessment;
using Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.Text.Json;

namespace ApplicationServices.Assessment
{
    public class AssessmentCollectService : BaseTenantService, IAssessmentCollectService
    {
        private readonly ILogger<AssessmentCollectService> _logger;
        private readonly IBaseTenantEntityRepository<AssessmentCollect> _AssessmentCollectRepository;
        private readonly IValidator<AssessmentCollectDto> _AssessmentCollectDtoValidator;
        private readonly IMapper _mapper;
        public AssessmentCollectService(
            ILogger<AssessmentCollectService> logger,
            IBaseTenantEntityRepository<AssessmentCollect> AssessmentCollectRepository,
            IValidator<AssessmentCollectDto> AssessmentCollectDtoValidator,
            IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
            _AssessmentCollectRepository = AssessmentCollectRepository.SetTenantId(TenantId);
            _AssessmentCollectDtoValidator = AssessmentCollectDtoValidator;
            _mapper = mapper;
        }
        public async Task<Result> AddAssessmentCollectAsync(AssessmentCollectDto AssessmentCollectDto)
        {
            try
            {
                var validationResult = await _AssessmentCollectDtoValidator.ValidateAsync(AssessmentCollectDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                await _AssessmentCollectRepository.Insert(_mapper.Map<AssessmentCollect>(AssessmentCollectDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail adding AssessmentCollect.");
                throw;
            }
        }
        public async Task<Result> EditAssessmentCollectAsync(AssessmentCollectDto AssessmentCollectDto)
        {
            try
            {
                var validationResult = await _AssessmentCollectDtoValidator.ValidateAsync(AssessmentCollectDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                if (!await _AssessmentCollectRepository.ExistsNotRemovedAsync(s => s.Id == AssessmentCollectDto.Id)) return new ErrorResult("AssessmentCollect not found.");

                await _AssessmentCollectRepository.Update(_mapper.Map<AssessmentCollect>(AssessmentCollectDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing AssessmentCollect.");
                throw;
            }
        }
        public async Task<Result> DeleteAssessmentCollectAsync(Guid AssessmentCollectId)
        {
            try
            {
                if (!await _AssessmentCollectRepository.ExistsNotRemovedAsync(s => s.Id == AssessmentCollectId)) return new ErrorResult("AssessmentCollect not found.");
                await _AssessmentCollectRepository.Delete(AssessmentCollectId);
                return new Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing AssessmentCollect.");
                throw;
            }
        }
        public async Task<Result<IEnumerable<AssessmentCollectGridDto>>> GetAssessmentCollectForGridAsync(string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction)
        {
            try
            {
                var query = _AssessmentCollectRepository.GetTenantDbSet()
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
                var result = await query.Select(c => new AssessmentCollectGridDto
                {
                    Name = c.Name,
                    ClientName = c.ClientName,
                    Id = c.Id,
                    CollectDate = c.CollectDate,
                    Details = c.Details,
                    FileName = c.FileName,
                    TechnicalResponsible = c.TechnicalResponsible,
                }).ToListAsync(); ;
                return new SuccessResult<IEnumerable<AssessmentCollectGridDto>>(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail getting AssessmentCollects.");
                throw;
            }
        }

        public async Task<Result> AddEncriptedCollectAsync(EncriptedCollectDto encriptedCollect)
        {
            try
            {
                var decriptedCollect = EncryptionUtils.DecryptString(encriptedCollect.EncriptedCollect, EncryptionUtils.GenerateTempSecureKey());
                var collectDto = JsonSerializer.Deserialize<AssessmentCollectDto>(decriptedCollect) ?? throw new InvalidCollectException("Fail to add collect.");

                await AddAssessmentCollectAsync(collectDto);
                return new AcceptedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail to add collect");
                throw;
            }
        }
    }
}
