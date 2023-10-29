using Core.DTOs.Authentication;
using Core.Results;

namespace Core.Interfaces.Services.Permissions
{
    public interface ISystemApiKeyService
    {
        Task<Result> AddSystemApiKeyAsync(SystemApiKeyDto contactDto);
        Task<Result> EditSystemApiKeyAsync(SystemApiKeyDto contactDto);
        Task<Result> DeleteSystemApiKeyAsync(Guid tenantId, Guid SystemApiKeytId);
        Task<Result<IEnumerable<SystemApiKeyDto>>> GetSystemApiKeyForGridAsync(Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction);
    }
}
