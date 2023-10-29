using Core.DTOs.Permissions;
using Core.Results;

namespace Core.Interfaces.Services.Permissions
{
    public interface IUserService
    {
        Task<Result> EditUserAsync(EditUserDto contactDto);
        Task<Result> DeleteUserAsync(Guid tenantId, Guid areatId);
        Task<Result<IEnumerable<UserGridDto>>> GetUserForGridAsync(Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction);
        Task<Result<string>> GetUserProfilePicAsync();
    }
}
