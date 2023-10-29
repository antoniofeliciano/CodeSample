using Core.DTOs.Permissions;
using Core.Results;

namespace Core.Interfaces.Services.Permissions
{
    public interface IInterfaceService
    {
        Task<Result> AddInterfaceAsync(InterfaceDto contactDto);
        Task<Result> EditInterfaceAsync(InterfaceDto contactDto);
        Task<Result> DeleteInterfaceAsync(Guid tenantId, Guid interfaceId);
        Task<Result<IEnumerable<InterfaceGridDto>>> GetInterfaceForGridAsync(Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction);
        Task<Result<IEnumerable<InterfaceDto>>> GetSystemInterfacesAsync();
    }
}
