using Api.Endpoints.Extensions.Auth;
using Api.Endpoints.Filters;
using Core.DTOs.Permissions;
using Core.Enumns;
using Core.Interfaces.Services.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Routes.Permissioning
{
    public static class AreasEndpoints
    {
        public static string InterfaceName => "Area";
        public static WebApplication AddAreaEndpoints(this WebApplication app)
        {
            app.MapGroup(InterfaceName).MapAreaEndpoints().AddEndpointFilter<ResultEndpointFilter>();
            return app;
        }

        private static RouteGroupBuilder MapAreaEndpoints(this RouteGroupBuilder mapper)
        {
            mapper.WithTags(InterfaceName)
                .MapPost("/", async ([FromServices] IAreaService areaService,
                [FromBody] AreaDto areaDto) => await areaService.AddAreaAsync(areaDto))
                .WithName("AddArea").RequirePermission(InterfaceName, PermissionType.create);

            mapper.WithTags(InterfaceName)
                .MapGet("/AreasForGrid", async ([FromServices] IAreaService areaService,
                [FromQuery] Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction) => await areaService.GetAreaForGridAsync(tenantId, searchTerm, pageNumber, pageSize, orderBy, direction))
                .WithName("AreasForGrid").RequirePermission(InterfaceName, PermissionType.read);

            mapper.WithTags(InterfaceName)
                .MapPut("/", async ([FromServices] IAreaService areaService,
                [FromBody] AreaDto areaDto) => await areaService.EditAreaAsync(areaDto))
                .WithName("EditArea").RequirePermission(InterfaceName, PermissionType.update);

            mapper.WithTags(InterfaceName)
                .MapDelete("/", async ([FromServices] IAreaService areaService,
                [FromQuery] Guid tenantId, Guid areaId) => await areaService.DeleteAreaAsync(tenantId, areaId))
                .WithName("DeleteArea").RequirePermission(InterfaceName, PermissionType.delete);

            return mapper;
        }

    }
}
