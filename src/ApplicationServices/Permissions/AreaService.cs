using ApplicationServices.Bases;
using AutoMapper;
using Core.DTOs.Permissions;
using Core.Entities.Permissions;
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
    public class AreaService : BaseTenantService, IAreaService
    {
        private readonly ILogger<AreaService> _logger;
        private readonly IBaseTenantEntityRepository<Area> _AreaRepository;
        private readonly IValidator<AreaDto> _AreaDtoValidator;
        private readonly IMapper _mapper;
        public AreaService(
            ILogger<AreaService> logger,
            IBaseTenantEntityRepository<Area> AreaRepository,
            IValidator<AreaDto> AreaDtoValidator,
            IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
            _AreaRepository = AreaRepository;
            _AreaDtoValidator = AreaDtoValidator;
            _mapper = mapper;
        }
        public async Task<Result> AddAreaAsync(AreaDto AreaDto)
        {
            try
            {
                var validationResult = await _AreaDtoValidator.ValidateAsync(AreaDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                await _AreaRepository.SetTenantId(AreaDto.TenantId).Insert(_mapper.Map<Area>(AreaDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail adding Area.");
                throw;
            }
        }
        public async Task<Result> EditAreaAsync(AreaDto AreaDto)
        {
            try
            {
                var validationResult = await _AreaDtoValidator.ValidateAsync(AreaDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                if (!(await _AreaRepository.SetTenantId(AreaDto.TenantId).ExistsNotRemovedAsync(s => s.Id == AreaDto.Id))) return new ErrorResult("Area not found.");

                await _AreaRepository.Update(_mapper.Map<Area>(AreaDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing Area.");
                throw;
            }
        }
        public async Task<Result> DeleteAreaAsync(Guid tenantId, Guid AreaId)
        {
            try
            {
                if (!(await _AreaRepository.SetTenantId(tenantId).ExistsNotRemovedAsync(s => s.Id == AreaId))) return new ErrorResult("Area not found.");
                await _AreaRepository.SetTenantId(tenantId).Delete(AreaId);
                return new Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing Area.");
                throw;
            }
        }
        public async Task<Result<IEnumerable<AreaDto>>> GetAreaForGridAsync(Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction)
        {
            try
            {
                var query = _AreaRepository.SetTenantId(tenantId).GetTenantDbSet()
                    .Where(a => a.RemovedAt == null)
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

                return new SuccessResult<IEnumerable<AreaDto>>(_mapper.Map<IEnumerable<AreaDto>>(await query.ToListAsync()));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail getting Areas.");
                throw;
            }
        }
    }
}
