using ApplicationServices.Bases;
using AutoMapper;
using Core.DTOs.Authentication;
using Core.Entities.Authentication;
using Core.Interfaces.Repositories.Bases;
using Core.Interfaces.Services.Permissions;
using Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace ApplicationServices.Permissions
{
    public class SystemApiKeyService : BaseTenantService, ISystemApiKeyService
    {
        private readonly ILogger<SystemApiKeyService> _logger;
        private readonly IBaseTenantEntityRepository<SystemApiKey> _SystemApiKeyRepository;
        private readonly IValidator<SystemApiKeyDto> _systemApiKeyDtoValidator;
        private readonly IMapper _mapper;
        public SystemApiKeyService(
            ILogger<SystemApiKeyService> logger,
            IBaseTenantEntityRepository<SystemApiKey> SystemApiKeyRepository,
            IValidator<SystemApiKeyDto> SystemApiKeyDtoValidator,
            IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
            _SystemApiKeyRepository = SystemApiKeyRepository;
            _systemApiKeyDtoValidator = SystemApiKeyDtoValidator;
            _mapper = mapper;
        }
        public async Task<Result> AddSystemApiKeyAsync(SystemApiKeyDto SystemApiKeyDto)
        {
            try
            {
                var validationResult = await _systemApiKeyDtoValidator.ValidateAsync(SystemApiKeyDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                await _SystemApiKeyRepository.SetTenantId(SystemApiKeyDto.TenantId).Insert(_mapper.Map<SystemApiKey>(SystemApiKeyDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail adding SystemApiKey.");
                throw;
            }
        }
        public async Task<Result> EditSystemApiKeyAsync(SystemApiKeyDto SystemApiKeyDto)
        {
            try
            {
                var validationResult = await _systemApiKeyDtoValidator.ValidateAsync(SystemApiKeyDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                if (!await _SystemApiKeyRepository.SetTenantId(SystemApiKeyDto.TenantId).ExistsNotRemovedAsync(s => s.Id == SystemApiKeyDto.Id)) return new ErrorResult("SystemApiKey not found.");

                await _SystemApiKeyRepository.Update(_mapper.Map<SystemApiKey>(SystemApiKeyDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing SystemApiKey.");
                throw;
            }
        }
        public async Task<Result> DeleteSystemApiKeyAsync(Guid tenantId, Guid SystemApiKeyId)
        {
            try
            {
                if (!await _SystemApiKeyRepository.SetTenantId(tenantId).ExistsNotRemovedAsync(s => s.Id == SystemApiKeyId)) return new ErrorResult("SystemApiKey not found.");
                await _SystemApiKeyRepository.SetTenantId(tenantId).Delete(SystemApiKeyId);
                return new Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing SystemApiKey.");
                throw;
            }
        }
        public async Task<Result<IEnumerable<SystemApiKeyDto>>> GetSystemApiKeyForGridAsync(Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction)
        {
            try
            {
                var query = _SystemApiKeyRepository.SetTenantId(tenantId).GetTenantDbSet()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(r => r.AppName.ToLower().Contains(searchTerm.ToLower()));
                }

                if (!string.IsNullOrEmpty(orderBy))
                {
                    query = query.OrderBy($"{orderBy} {(direction == "desc" ? "descending" : "ascending")}");
                }
                else
                {
                    query = query.OrderBy($"AppName {(direction == "desc" ? "descending" : "ascending")}");
                }

                if (pageNumber.HasValue && pageSize.HasValue)
                {
                    query = query.Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
                }

                return new SuccessResult<IEnumerable<SystemApiKeyDto>>(_mapper.Map<IEnumerable<SystemApiKeyDto>>(await query.ToListAsync()));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail getting SystemApiKeys.");
                throw;
            }
        }
    }
}
