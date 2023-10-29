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
    public class PermissionService : BaseTenantService, IPermissionService
    {
        private readonly ILogger<PermissionService> _logger;
        private readonly IBaseTenantEntityRepository<Permission> _PermissionRepository;
        private readonly IValidator<PermissionDto> _PermissionDtoValidator;
        private readonly IMapper _mapper;
        public PermissionService(
            ILogger<PermissionService> logger,
            IBaseTenantEntityRepository<Permission> PermissionRepository,
            IValidator<PermissionDto> PermissionDtoValidator,
            IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
            _PermissionRepository = PermissionRepository;
            _PermissionDtoValidator = PermissionDtoValidator;
            _mapper = mapper;
        }
        public async Task<Result> AddPermissionAsync(PermissionDto PermissionDto)
        {
            try
            {
                var validationResult = await _PermissionDtoValidator.ValidateAsync(PermissionDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                await _PermissionRepository.SetTenantId(PermissionDto.TenantId).Insert(_mapper.Map<Permission>(PermissionDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail adding Permission.");
                throw;
            }
        }
        public async Task<Result> EditPermissionAsync(PermissionDto PermissionDto)
        {
            try
            {
                var validationResult = await _PermissionDtoValidator.ValidateAsync(PermissionDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                if (!(await _PermissionRepository.SetTenantId(PermissionDto.TenantId).ExistsNotRemovedAsync(s => s.Id == PermissionDto.Id))) return new ErrorResult("Permission not found.");

                await _PermissionRepository.Update(_mapper.Map<Permission>(PermissionDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing Permission.");
                throw;
            }
        }
        public async Task<Result> DeletePermissionAsync(Guid tenantId, Guid PermissionId)
        {
            try
            {
                if (!(await _PermissionRepository.SetTenantId(tenantId).ExistsNotRemovedAsync(s => s.Id == PermissionId))) return new ErrorResult("Permission not found.");
                await _PermissionRepository.SetTenantId(tenantId).Delete(PermissionId);
                return new Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing Permission.");
                throw;
            }
        }
        public async Task<Result<IEnumerable<PermissionDto>>> GetPermissionForGridAsync(Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction)
        {
            try
            {
                var query = _PermissionRepository.SetTenantId(tenantId).GetTenantDbSet()
                    .Include(i => i.Interface).ThenInclude(a => a.Area)
                    .Include(r => r.Role)
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

                return new SuccessResult<IEnumerable<PermissionDto>>(_mapper.Map<IEnumerable<PermissionDto>>(await query.ToListAsync()));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail getting Permissions.");
                throw;
            }
        }

    }
}
