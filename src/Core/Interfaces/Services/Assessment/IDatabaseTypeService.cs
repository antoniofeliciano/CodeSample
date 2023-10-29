using Core.DTOs.Assessment;
using Core.Results;

namespace Core.Interfaces.Services.Assessment
{
    public interface IDatabaseTypeService
    {
        Task<Result> AddDatabaseTypeAsync(DatabaseTypeDto contactDto);
        Task<Result> EditDatabaseTypeAsync(DatabaseTypeDto contactDto);
        Task<Result> DeleteDatabaseTypeAsync(Guid databaseTypetId);
        Task<Result<IEnumerable<DatabaseTypeDto>>> GetDatabaseTypeForGridAsync(string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction);
    }
}
