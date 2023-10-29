using Api.Endpoints.Extensions.Auth;
using Api.Endpoints.Filters;
using Core.DTOs.Authentication;
using Core.Enumns;
using Core.Interfaces.Services.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Routes.Permissioning
{
    public static class SystemApiKeysEndpoints
    {
        public static string InterfaceName => "SystemApiKey";
        public static WebApplication AddSystemApiKeyEndpoints(this WebApplication app)
        {
            app.MapGroup(InterfaceName).MapSystemApiKeyEndpoints().AddEndpointFilter<ResultEndpointFilter>();
            return app;
        }

        private static RouteGroupBuilder MapSystemApiKeyEndpoints(this RouteGroupBuilder mapper)
        {
            mapper.WithTags(InterfaceName)
                .MapPost("/", async ([FromServices] ISystemApiKeyService systemApiKeyService,
                [FromBody] SystemApiKeyDto systemApiKeyDto) => await systemApiKeyService.AddSystemApiKeyAsync(systemApiKeyDto))
                .WithName("AddSystemApiKey").RequirePermission(InterfaceName, PermissionType.create);

            mapper.WithTags(InterfaceName)
                .MapGet("/SystemApiKeysForGrid", async ([FromServices] ISystemApiKeyService systemApiKeyService,
                [FromQuery] Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction) => await systemApiKeyService.GetSystemApiKeyForGridAsync(tenantId, searchTerm, pageNumber, pageSize, orderBy, direction))
                .WithName("SystemApiKeysForGrid").RequirePermission(InterfaceName, PermissionType.read);

            mapper.WithTags(InterfaceName)
                .MapPut("/", async ([FromServices] ISystemApiKeyService systemApiKeyService,
                [FromBody] SystemApiKeyDto systemApiKeyDto) => await systemApiKeyService.EditSystemApiKeyAsync(systemApiKeyDto))
                .WithName("EditSystemApiKey").RequirePermission(InterfaceName, PermissionType.update);

            mapper.WithTags(InterfaceName)
                .MapDelete("/", async ([FromServices] ISystemApiKeyService systemApiKeyService,
                [FromQuery] Guid tenantId, Guid systemApiKeyId) => await systemApiKeyService.DeleteSystemApiKeyAsync(tenantId, systemApiKeyId))
                .WithName("DeleteSystemApiKey").RequirePermission(InterfaceName, PermissionType.delete);

            return mapper;
        }

    }
}
