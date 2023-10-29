using Api.Endpoints.Filters;
using Core.DTOs.Authentication;
using Core.Interfaces.Services.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Api.Endpoints.Routes.Authentication
{
    public static class AuthenticationEndpoints
    {
        public static string InterfaceName => "Authentication";
        public static WebApplication AddAuthenticationEndpoints(this WebApplication app)
        {
            app.MapGroup(InterfaceName).MapAuthenticationEndpoints().AddEndpointFilter<ResultEndpointFilter>();
            return app;
        }

        private static RouteGroupBuilder MapAuthenticationEndpoints(this RouteGroupBuilder mapper)
        {
            mapper.WithTags(InterfaceName)
                .MapPost("/SignIn", async ([FromServices] IAuthenticationService authenticationService, [FromBody] LoginDto login) => await authenticationService.SignInAsync(login))
                .WithName("SignIn");

            mapper.WithTags(InterfaceName)
                .MapPost("/RefreshToken", async ([FromServices] IAuthenticationService authenticationService, [FromBody] RefreshTokenDto refreshTokenDto) => await authenticationService.RefreshTokenAsync(refreshTokenDto))
                .WithName("RefreshToken");

            return mapper;
        }

    }
}
