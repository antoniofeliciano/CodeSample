using Core.DTOs.Tenants;
using Core.Results;

namespace Core.Interfaces.Services.Tenants
{
    public interface ITenantService
    {
        Task<Result> AddTenantAsync(TenantDto TenantDto);
        Task<Result> EditTenantAsync(TenantDto TenantDto);
        Task<Result> DeleteTenantAsync(Guid TenantId);
        Task<Result<IEnumerable<TenantGridDto>>> GetTenantForGridAsync(string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction);
        Task<Result<TenantDto>> GetTenantDataAsync();
    }
}
