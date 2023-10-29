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
    public class DatabaseTypeService : BaseTenantService, IDatabaseTypeService
    {
        private readonly ILogger<DatabaseTypeService> _logger;
        private readonly IBaseEntityRepository<DatabaseType> _databaseTypeRepository;
        private readonly IValidator<DatabaseTypeDto> _DatabaseTypeDtoValidator;
        private readonly IMapper _mapper;
        public DatabaseTypeService(
            ILogger<DatabaseTypeService> logger,
            IBaseEntityRepository<DatabaseType> DatabaseTypeRepository,
            IValidator<DatabaseTypeDto> DatabaseTypeDtoValidator,
            IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
            _databaseTypeRepository = DatabaseTypeRepository;
            _DatabaseTypeDtoValidator = DatabaseTypeDtoValidator;
            _mapper = mapper;
        }
        public async Task<Result> AddDatabaseTypeAsync(DatabaseTypeDto DatabaseTypeDto)
        {
            try
            {
                var validationResult = await _DatabaseTypeDtoValidator.ValidateAsync(DatabaseTypeDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                await _databaseTypeRepository.Insert(_mapper.Map<DatabaseType>(DatabaseTypeDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail adding DatabaseType.");
                throw;
            }
        }
        public async Task<Result> EditDatabaseTypeAsync(DatabaseTypeDto DatabaseTypeDto)
        {
            try
            {
                var validationResult = await _DatabaseTypeDtoValidator.ValidateAsync(DatabaseTypeDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                if (!await _databaseTypeRepository.ExistsNotRemovedAsync(s => s.Id == DatabaseTypeDto.Id)) return new ErrorResult("DatabaseType not found.");

                await _databaseTypeRepository.Update(_mapper.Map<DatabaseType>(DatabaseTypeDto));
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing DatabaseType.");
                throw;
            }
        }
        public async Task<Result> DeleteDatabaseTypeAsync(Guid DatabaseTypeId)
        {
            try
            {
                if (!await _databaseTypeRepository.ExistsNotRemovedAsync(s => s.Id == DatabaseTypeId)) return new ErrorResult("DatabaseType not found.");
                await _databaseTypeRepository.Delete(DatabaseTypeId);
                return new Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing DatabaseType.");
                throw;
            }
        }
        public async Task<Result<IEnumerable<DatabaseTypeDto>>> GetDatabaseTypeForGridAsync(string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction)
        {
            try
            {
                var query = _databaseTypeRepository.GetDbSet()
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

                return new SuccessResult<IEnumerable<DatabaseTypeDto>>(_mapper.Map<IEnumerable<DatabaseTypeDto>>(await query.ToListAsync()));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail getting Assessment Queries.");
                throw;
            }
        }
    }
}
