using Api.Endpoints.Extensions.Auth;
using Api.Endpoints.Filters;
using Core.DTOs.Tenants;
using Core.Enumns;
using Core.Interfaces.Services.Tenants;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Routes.Permissioning
{
    public static class TenantsEndpoints
    {
        public static string InterfaceName => "Tenant";
        public static WebApplication AddTenantsEndpoints(this WebApplication app)
        {
            app.MapGroup(InterfaceName).MapTenantsEndpoints().AddEndpointFilter<ResultEndpointFilter>();
            return app;
        }
        private static RouteGroupBuilder MapTenantsEndpoints(this RouteGroupBuilder mapper)
        {
            mapper.WithTags(InterfaceName)
                .MapPost("/", async ([FromServices] ITenantService tenantService,
                [FromBody] TenantDto tenantDto) => await tenantService.AddTenantAsync(tenantDto))
                .WithName("AddTenant").RequirePermission(InterfaceName, PermissionType.create);

            mapper.WithTags(InterfaceName)
                .MapGet("/TenantsForGrid", async ([FromServices] ITenantService tenantService,
                [FromQuery] string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction) => await tenantService.GetTenantForGridAsync(searchTerm, pageNumber, pageSize, orderBy, direction))
                .WithName("TenantsForGrid");

            mapper.WithTags(InterfaceName)
                .MapPut("/", async ([FromServices] ITenantService tenantService,
                [FromBody] TenantDto tenantDto) => await tenantService.EditTenantAsync(tenantDto))
                .WithName("EditTenant").RequirePermission(InterfaceName, PermissionType.update);

            mapper.WithTags(InterfaceName)
                .MapDelete("/", async ([FromServices] ITenantService tenantService,
                [FromQuery] Guid tenantId) => await tenantService.DeleteTenantAsync(tenantId))
                .WithName("DeleteTenant").RequirePermission(InterfaceName, PermissionType.delete);

            mapper.WithTags(InterfaceName)
                .MapGet("/TenantData", async ([FromServices] ITenantService tenantService) => await tenantService.GetTenantDataAsync())
                .WithName("TenantData").RequireSignedUser();

            return mapper;
        }

    }
}
