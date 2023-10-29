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
    public class RoleService : BaseTenantService, IRoleService
    {
        private readonly ILogger<RoleService> _logger;
        private readonly IBaseTenantEntityRepository<Role> _RoleRepository;
        private readonly IValidator<RoleDto> _RoleDtoValidator;
        private readonly IMapper _mapper;
        public RoleService(
            ILogger<RoleService> logger,
            IBaseTenantEntityRepository<Role> RoleRepository,
            IValidator<RoleDto> RoleDtoValidator,
            IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
            _RoleRepository = RoleRepository;
            _RoleDtoValidator = RoleDtoValidator;
            _mapper = mapper;
        }
        public async Task<Result> AddRoleAsync(RoleDto RoleDto)
        {
            try
            {
                var validationResult = await _RoleDtoValidator.ValidateAsync(RoleDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                await _RoleRepository.SetTenantId(RoleDto.TenantId).Insert(_mapper.Map<Role>(RoleDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail adding Role.");
                throw;
            }
        }
        public async Task<Result> EditRoleAsync(RoleDto RoleDto)
        {
            try
            {
                var validationResult = await _RoleDtoValidator.ValidateAsync(RoleDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                if (!await _RoleRepository.SetTenantId(RoleDto.TenantId).ExistsNotRemovedAsync(s => s.Id == RoleDto.Id)) return new ErrorResult("Role not found.");

                await _RoleRepository.Update(_mapper.Map<Role>(RoleDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing Role.");
                throw;
            }
        }
        public async Task<Result> DeleteRoleAsync(Guid tenantId, Guid RoleId)
        {
            try
            {
                if (!await _RoleRepository.SetTenantId(tenantId).ExistsNotRemovedAsync(s => s.Id == RoleId)) return new ErrorResult("Role not found.");
                await _RoleRepository.SetTenantId(tenantId).Delete(RoleId);
                return new Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing Role.");
                throw;
            }
        }
        public async Task<Result<IEnumerable<RoleDto>>> GetRoleForGridAsync(Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction)
        {
            try
            {
                var query = _RoleRepository.SetTenantId(tenantId).GetTenantDbSet()
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

                return new SuccessResult<IEnumerable<RoleDto>>(_mapper.Map<IEnumerable<RoleDto>>(await query.ToListAsync()));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail getting Roles.");
                throw;
            }
        }
    }
}
