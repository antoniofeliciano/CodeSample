using Core.DTOs.Permissions;
using Core.Results;

namespace Core.Interfaces.Services.Permissions
{
    public interface IPermissionService
    {
        Task<Result> AddPermissionAsync(PermissionDto contactDto);
        Task<Result> EditPermissionAsync(PermissionDto contactDto);
        Task<Result> DeletePermissionAsync(Guid tenantId, Guid permissiontId);
        Task<Result<IEnumerable<PermissionDto>>> GetPermissionForGridAsync(Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction);

    }
}
