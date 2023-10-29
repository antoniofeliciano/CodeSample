using ApplicationServices.Bases;
using AutoMapper;
using Core.DTOs.Tenants;
using Core.Entities.Bases;
using Core.Entities.Permissions;
using Core.Entities.Tenants;
using Core.Exceptions;
using Core.Interfaces.Repositories.Bases;
using Core.Interfaces.Services.Tenants;
using Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;

namespace ApplicationServices.Tenants
{
    public class TenantService : BaseTenantService, ITenantService
    {
        private readonly ILogger<TenantService> _logger;
        private readonly IBaseEntityRepository<Tenant> _tenantRepository;
        private readonly IBaseTenantEntityRepository<Permission> _permissionRepository;
        private readonly IValidator<TenantDto> _tenantDtoValidator;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMapper _mapper;

        public TenantService(
            ILogger<TenantService> logger,
            IBaseEntityRepository<Tenant> tenantRepository,
            IBaseTenantEntityRepository<Permission> permissionRepository,
            IValidator<TenantDto> tenantDtoValidator,
            IHttpContextAccessor contextAccessor,
            IMapper mapper) : base(contextAccessor)
        {
            _logger = logger;
            _tenantRepository = tenantRepository;
            _permissionRepository = permissionRepository;
            _tenantDtoValidator = tenantDtoValidator;
            _contextAccessor = contextAccessor;
            _mapper = mapper;
        }
        public async Task<Result> AddTenantAsync(TenantDto tenantDto)
        {
            try
            {
                var validationResult = await _tenantDtoValidator.ValidateAsync(tenantDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                await _tenantRepository.Insert(_mapper.Map<Tenant>(tenantDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail adding tenant.");
                throw;
            }
        }
        public async Task<Result> EditTenantAsync(TenantDto tenantDto)
        {
            try
            {
                var validationResult = await _tenantDtoValidator.ValidateAsync(tenantDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                if (!(await _tenantRepository.ExistsNotRemovedAsync(s => s.Id == tenantDto.Id))) return new ErrorResult("Tenant not found.");

                await _tenantRepository.Update(_mapper.Map<Tenant>(tenantDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing tenant.");
                throw;
            }
        }
        public async Task<Result> DeleteTenantAsync(Guid tenantId)
        {
            try
            {
                if (!(await _tenantRepository.ExistsNotRemovedAsync(s => s.Id == tenantId))) return new ErrorResult("Tenant not found.");
                await _tenantRepository.Delete(tenantId);
                return new Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing tenant.");
                throw;
            }
        }
        public async Task<Result<IEnumerable<TenantGridDto>>> GetTenantForGridAsync(string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction)
        {
            try
            {
                var query = _tenantRepository.GetDbSet()
                    .AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLowerInvariant();
                    query = query.Where(r => r.Name.ToLowerInvariant().Contains(searchTerm.ToLowerInvariant()));
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

                return new SuccessResult<IEnumerable<TenantGridDto>>(_mapper.Map<IEnumerable<TenantGridDto>>(await query.ToListAsync()));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail getting tenants.");
                throw;
            }
        }

        public async Task<Result<TenantDto>> GetTenantDataAsync()
        {

            var tenant = await _tenantRepository.GetByIdAsync(TenantId);
            if (tenant is null) return new NotFoundResult<TenantDto>();
            var tenantData = _mapper.Map<TenantDto>(tenant);

            tenantData.Menu = _permissionRepository
                .SetTenantId(TenantId)
                .GetTenantDbSet()
                .Include(x => x.Interface).ThenInclude(a => a.Area)
                .Where(r => r.RoleId == RoleId)
                .Where(p => p.RemovedAt == null)
                .Where(p => p.CanRead)
                .GroupBy(c => c.Interface!.Area!)
                .Select(a => new
                {
                    type = "collapse",
                    a.Key.Position,
                    a.Key.Name,
                    Key = a.Key.Name.ToLowerInvariant(),
                    a.Key.Icon,
                    a.Key.Visible,
                    Collapse = a.Select(i => new
                    {
                        i.Interface.Name,
                        Key = i.Interface.SystemName.ToLowerInvariant(),
                        route = $"/{a.Key.Name}{i.Interface.Route}".ToLowerInvariant(),
                        Component = i.Interface.SystemName.ToLowerInvariant(),
                        i.Interface.Visible,
                        i.Interface.Renderable
                    })

                })
                .OrderBy(a => a.Position);


            return new SuccessResult<TenantDto>(tenantData);
        }
    }
}
