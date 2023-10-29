using Core.DTOs.Authentication;
using Core.Results;

namespace Core.Interfaces.Services.Permissions
{
    public interface IRoleService
    {
        Task<Result> AddRoleAsync(RoleDto contactDto);
        Task<Result> EditRoleAsync(RoleDto contactDto);
        Task<Result> DeleteRoleAsync(Guid tenantId, Guid roletId);
        Task<Result<IEnumerable<RoleDto>>> GetRoleForGridAsync(Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction);
    }
}
