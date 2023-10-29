using Core.DTOs.Permissions;
using Core.Results;

namespace Core.Interfaces.Services.Permissions
{
    public interface IAreaService
    {
        Task<Result> AddAreaAsync(AreaDto contactDto);
        Task<Result> EditAreaAsync(AreaDto contactDto);
        Task<Result> DeleteAreaAsync(Guid tenantId, Guid areatId);
        Task<Result<IEnumerable<AreaDto>>> GetAreaForGridAsync(Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction);
    }
}
