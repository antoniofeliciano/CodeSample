using Api.Endpoints.Extensions.Auth;
using Api.Endpoints.Filters;
using Core.DTOs.Assessment;
using Core.Enumns;
using Core.Interfaces.Services.Assessment;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Routes.Assessment
{
    public static class DatabaseTypeEndpoints
    {
        public static string InterfaceName => "DatabaseType";
        public static WebApplication AddDatabaseTypeEndpoints(this WebApplication app)
        {
            app.MapGroup(InterfaceName).MapDatabaseTypeEndpoints().AddEndpointFilter<ResultEndpointFilter>();
            return app;
        }

        private static RouteGroupBuilder MapDatabaseTypeEndpoints(this RouteGroupBuilder mapper)
        {
            mapper.WithTags(InterfaceName)
                .MapPost("/", async ([FromServices] IDatabaseTypeService databaseTypeService,
                [FromBody] DatabaseTypeDto databaseTypeDto) => await databaseTypeService.AddDatabaseTypeAsync(databaseTypeDto))
                .WithName("AddDatabaseType").RequirePermission(InterfaceName, PermissionType.create);

            mapper.WithTags(InterfaceName)
                .MapGet("/DatabaseTypesForGrid", async ([FromServices] IDatabaseTypeService databaseTypeService,
                [FromQuery] string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction) => await databaseTypeService.GetDatabaseTypeForGridAsync(searchTerm, pageNumber, pageSize, orderBy, direction))
                .WithName("DatabaseTypesForGrid").RequirePermission(InterfaceName, PermissionType.read);

            mapper.WithTags(InterfaceName)
                .MapPut("/", async ([FromServices] IDatabaseTypeService databaseTypeService,
                [FromBody] DatabaseTypeDto databaseTypeDto) => await databaseTypeService.EditDatabaseTypeAsync(databaseTypeDto))
                .WithName("EditDatabaseType").RequirePermission(InterfaceName, PermissionType.update);

            mapper.WithTags(InterfaceName)
                .MapDelete("/", async ([FromServices] IDatabaseTypeService databaseTypeService,
                [FromQuery] Guid databaseTypeId) => await databaseTypeService.DeleteDatabaseTypeAsync(databaseTypeId))
                .WithName("DeleteDatabaseType").RequirePermission(InterfaceName, PermissionType.delete);

            return mapper;
        }

    }
}
