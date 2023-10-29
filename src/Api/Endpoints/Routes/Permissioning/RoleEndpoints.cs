using Api.Endpoints.Extensions.Auth;
using Api.Endpoints.Filters;
using Core.DTOs.Authentication;
using Core.Enumns;
using Core.Interfaces.Services.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Routes.Permissioning
{
    public static class RolesEndpoints
    {
        public static string InterfaceName => "SystemRole";
        public static WebApplication AddRoleEndpoints(this WebApplication app)
        {
            app.MapGroup(InterfaceName).MapRoleEndpoints().AddEndpointFilter<ResultEndpointFilter>();
            return app;
        }

        private static RouteGroupBuilder MapRoleEndpoints(this RouteGroupBuilder mapper)
        {
            mapper.WithTags(InterfaceName)
                .MapPost("/", async ([FromServices] IRoleService roleService,
                [FromBody] RoleDto roleDto) => await roleService.AddRoleAsync(roleDto))
                .WithName("AddRole").RequirePermission(InterfaceName, PermissionType.create);

            mapper.WithTags(InterfaceName)
                .MapGet("/RolesForGrid", async ([FromServices] IRoleService roleService,
                [FromQuery] Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction) => await roleService.GetRoleForGridAsync(tenantId, searchTerm, pageNumber, pageSize, orderBy, direction))
                .WithName("RolesForGrid").RequirePermission(InterfaceName, PermissionType.read);

            mapper.WithTags(InterfaceName)
                .MapPut("/", async ([FromServices] IRoleService roleService,
                [FromBody] RoleDto roleDto) => await roleService.EditRoleAsync(roleDto))
                .WithName("EditRole").RequirePermission(InterfaceName, PermissionType.update);

            mapper.WithTags(InterfaceName)
                .MapDelete("/", async ([FromServices] IRoleService roleService,
                [FromQuery] Guid tenantId, Guid roleId) => await roleService.DeleteRoleAsync(tenantId, roleId))
                .WithName("DeleteRole").RequirePermission(InterfaceName, PermissionType.delete);

            return mapper;
        }

    }
}
