using Api.Endpoints.Extensions.Auth;
using Api.Endpoints.Filters;
using Core.DTOs.Permissions;
using Core.Enumns;
using Core.Interfaces.Services.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Routes.Permissioning
{
    public static class InterfacesEndpoints
    {
        public static string InterfaceName => "SystemInterface";
        public static WebApplication AddInterfaceEndpoints(this WebApplication app)
        {
            app.MapGroup(InterfaceName).MapInterfaceEndpoints().AddEndpointFilter<ResultEndpointFilter>();
            return app;
        }

        private static RouteGroupBuilder MapInterfaceEndpoints(this RouteGroupBuilder mapper)
        {
            mapper.WithTags(InterfaceName)
                .MapGet("/SystemInterfaces", async ([FromServices] IInterfaceService interfaceService) => await interfaceService.GetSystemInterfacesAsync())
                .WithName("SystemInterfaces");

            mapper.WithTags(InterfaceName)
                .MapPost("/", async ([FromServices] IInterfaceService interfaceService,
                [FromBody] InterfaceDto interfaceDto) => await interfaceService.AddInterfaceAsync(interfaceDto))
                .WithName("AddInterface").RequirePermission(InterfaceName, PermissionType.create);

            mapper.WithTags(InterfaceName)
                .MapGet("/InterfacesForGrid", async ([FromServices] IInterfaceService interfaceService,
                [FromQuery] Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction) => await interfaceService.GetInterfaceForGridAsync(tenantId, searchTerm, pageNumber, pageSize, orderBy, direction))
                .WithName("InterfacesForGrid").RequirePermission(InterfaceName, PermissionType.read);

            mapper.WithTags(InterfaceName)
                .MapPut("/", async ([FromServices] IInterfaceService interfaceService,
                [FromBody] InterfaceDto interfaceDto) => await interfaceService.EditInterfaceAsync(interfaceDto))
                .WithName("EditInterface").RequirePermission(InterfaceName, PermissionType.update);

            mapper.WithTags(InterfaceName)
                .MapDelete("/", async ([FromServices] IInterfaceService interfaceService,
                [FromQuery] Guid tenantId, Guid interfaceId) => await interfaceService.DeleteInterfaceAsync(tenantId, interfaceId))
                .WithName("DeleteInterface").RequirePermission(InterfaceName, PermissionType.delete);

            return mapper;
        }

    }
}
