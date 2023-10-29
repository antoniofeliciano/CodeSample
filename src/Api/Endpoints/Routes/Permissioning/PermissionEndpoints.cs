using Api.Endpoints.Extensions.Auth;
using Api.Endpoints.Filters;
using Core.DTOs.Permissions;
using Core.Enumns;
using Core.Interfaces.Services.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Routes.Permissioning
{
    public static class PermissionsEndpoints
    {
        public static string InterfaceName => "Permission";
        public static WebApplication AddPermissionEndpoints(this WebApplication app)
        {
            app.MapGroup(InterfaceName).MapPermissionEndpoints().AddEndpointFilter<ResultEndpointFilter>();
            return app;
        }

        private static RouteGroupBuilder MapPermissionEndpoints(this RouteGroupBuilder mapper)
        {
            mapper.WithTags(InterfaceName)
                .MapPost("/", async ([FromServices] IPermissionService permissionService,
                [FromBody] PermissionDto permissionDto) => await permissionService.AddPermissionAsync(permissionDto))
                .WithName("AddPermission").RequirePermission(InterfaceName, PermissionType.create);

            mapper.WithTags(InterfaceName)
                .MapGet("/PermissionsForGrid", async ([FromServices] IPermissionService permissionService,
                [FromQuery] Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction) => await permissionService.GetPermissionForGridAsync(tenantId, searchTerm, pageNumber, pageSize, orderBy, direction))
                .WithName("PermissionsForGrid").RequirePermission(InterfaceName, PermissionType.read);

            mapper.WithTags(InterfaceName)
                .MapPut("/", async ([FromServices] IPermissionService permissionService,
                [FromBody] PermissionDto permissionDto) => await permissionService.EditPermissionAsync(permissionDto))
                .WithName("EditPermission").RequirePermission(InterfaceName, PermissionType.update);

            mapper.WithTags(InterfaceName)
                .MapDelete("/", async ([FromServices] IPermissionService permissionService,
                [FromQuery] Guid tenantId, Guid permissionId) => await permissionService.DeletePermissionAsync(tenantId, permissionId))
                .WithName("DeletePermission").RequirePermission(InterfaceName, PermissionType.delete);

            return mapper;
        }

    }
}
