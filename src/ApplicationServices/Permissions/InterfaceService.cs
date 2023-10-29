using ApplicationServices.Bases;
using AutoMapper;
using Core.DTOs.Permissions;
using Core.Entities.Permissions;
using Core.Helpers;
using Core.Interfaces.Repositories.Bases;
using Core.Interfaces.Services.Permissions;
using Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace ApplicationServices.Permissions
{
    public class InterfaceService : BaseTenantService, IInterfaceService
    {
        private readonly ILogger<InterfaceService> _logger;
        private readonly IBaseTenantEntityRepository<Interface> _InterfaceRepository;
        private readonly IValidator<InterfaceDto> _InterfaceDtoValidator;
        private readonly IMapper _mapper;
        public InterfaceService(
            ILogger<InterfaceService> logger,
            IBaseTenantEntityRepository<Interface> InterfaceRepository,
            IValidator<InterfaceDto> InterfaceDtoValidator,
            IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
            _InterfaceRepository = InterfaceRepository;
            _InterfaceDtoValidator = InterfaceDtoValidator;
            _mapper = mapper;
        }

        public async Task<Result> AddInterfaceAsync(InterfaceDto InterfaceDto)
        {
            try
            {
                var validationResult = await _InterfaceDtoValidator.ValidateAsync(InterfaceDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                await _InterfaceRepository.SetTenantId(InterfaceDto.TenantId).Insert(_mapper.Map<Interface>(InterfaceDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail adding Interface.");
                throw;
            }
        }
        public async Task<Result> EditInterfaceAsync(InterfaceDto InterfaceDto)
        {
            try
            {
                var validationResult = await _InterfaceDtoValidator.ValidateAsync(InterfaceDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                if (!(await _InterfaceRepository.SetTenantId(InterfaceDto.TenantId).ExistsNotRemovedAsync(s => s.Id == InterfaceDto.Id))) return new ErrorResult("Interface not found.");

                await _InterfaceRepository.Update(_mapper.Map<Interface>(InterfaceDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing Interface.");
                throw;
            }
        }
        public async Task<Result> DeleteInterfaceAsync(Guid tenantId, Guid InterfaceId)
        {
            try
            {
                if (!(await _InterfaceRepository.SetTenantId(tenantId).ExistsNotRemovedAsync(s => s.Id == InterfaceId))) return new ErrorResult("Interface not found.");
                await _InterfaceRepository.SetTenantId(tenantId).Delete(InterfaceId);
                return new Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing Interface.");
                throw;
            }
        }
        public async Task<Result<IEnumerable<InterfaceGridDto>>> GetInterfaceForGridAsync(Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction)
        {
            try
            {
                var query = _InterfaceRepository.SetTenantId(tenantId).GetTenantDbSet()
                    .Include(a => a.Area)
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

                return new SuccessResult<IEnumerable<InterfaceGridDto>>(_mapper.Map<IEnumerable<InterfaceGridDto>>(await query.ToListAsync()));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail getting Interfaces.");
                throw;
            }
        }
        public async Task<Result<IEnumerable<InterfaceDto>>> GetSystemInterfacesAsync()
        {
            try
            {
                var systemInterfaces = Assembly.GetEntryAssembly()!.GetTypes()
                    .Where(t => (t.Namespace ?? "").StartsWith("Api.Endpoints.Routes") && t.IsClass && t.IsSealed && t.IsAbstract).ToList()
                    .Select(e =>
                    {
                        var tagname = e.GetProperty("InterfaceName")!.GetValue(e)!.ToString()!;
                        return new InterfaceDto
                        {
                            Name = tagname,
                            SystemName = tagname.ToKebabCase(),
                            Route = $"/{tagname.ToKebabCase()}",
                        };
                    });

                return new SuccessResult<IEnumerable<InterfaceDto>>(await Task.FromResult(systemInterfaces));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail getting user informations.");
                throw;
            }
        }
    }
}
