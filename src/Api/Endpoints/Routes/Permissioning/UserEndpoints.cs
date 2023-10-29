using Api.Endpoints.Extensions.Auth;
using Api.Endpoints.Filters;
using Core.DTOs.Permissions;
using Core.Enumns;
using Core.Interfaces.Services.Authentication;
using Core.Interfaces.Services.Permissions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Routes.Permissioning
{
    public static class UsersEndpoints
    {
        public static string InterfaceName => "User";
        public static WebApplication AddUserEndpoints(this WebApplication app)
        {
            app.MapGroup(InterfaceName).MapUserEndpoints().AddEndpointFilter<ResultEndpointFilter>();
            return app;
        }

        private static RouteGroupBuilder MapUserEndpoints(this RouteGroupBuilder mapper)
        {
            mapper.WithTags(InterfaceName)
                .MapPost("/", async ([FromServices] IAuthenticationService authenticationService,
                [FromBody] CreateUserDto userDto) => await authenticationService.CreateUserAsync(userDto))
                .WithName("AddUser").RequirePermission(InterfaceName, PermissionType.create);

            mapper.WithTags(InterfaceName)
                .MapGet("/UsersForGrid", async ([FromServices] IUserService userService,
                [FromQuery] Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction) => await userService.GetUserForGridAsync(tenantId, searchTerm, pageNumber, pageSize, orderBy, direction))
                .WithName("UsersForGrid").RequirePermission(InterfaceName, PermissionType.read);

            mapper.WithTags(InterfaceName)
                .MapPut("/", async ([FromServices] IUserService userService,
                [FromBody] EditUserDto userDto) => await userService.EditUserAsync(userDto))
                .WithName("EditUser").RequirePermission(InterfaceName, PermissionType.update);

            mapper.WithTags(InterfaceName)
                .MapDelete("/", async ([FromServices] IUserService userService,
                [FromQuery] Guid tenantId, Guid userId) => await userService.DeleteUserAsync(tenantId, userId))
                .WithName("DeleteUser").RequirePermission(InterfaceName, PermissionType.delete);


            mapper.WithTags(InterfaceName)
                .MapGet("/ProfilePicture", async ([FromServices] IUserService userService) => await userService.GetUserProfilePicAsync())
                .WithName("ProfilePicture").RequireSignedUser();

            return mapper;
        }

    }
}
