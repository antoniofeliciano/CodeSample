using Api.Endpoints.Filters;
using Core.Enumns;
using Core.Helpers;

namespace Api.Endpoints.Extensions.Auth
{
    public static class AuthorizationExtensions
    {
        public static RouteHandlerBuilder RequirePermission(this RouteHandlerBuilder builder, string interfaceName, PermissionType permission)
        {
            return builder.RequireAuthorization(p =>
            {
                p.RequireAssertion(context =>
                {
                    if (context.User.HasClaim(c => c.Type == interfaceName.ToKebabCase()))
                    {
                        var claim = context.User.FindFirst(c => c.Type == interfaceName.ToKebabCase());
                        return claim!.Value.Contains(permission.ToString().First()) || claim!.Value.Equals("a");
                    }
                    return false;
                });
            });
        }
        public static RouteHandlerBuilder RequireApiKey(this RouteHandlerBuilder builder)
        {
            return builder.RequireAuthorization(p =>
            {
                p.AddRequirements(new ApiKeyAuthentication());
            });
        }
        public static RouteHandlerBuilder RequireSignedUser(this RouteHandlerBuilder builder)
        {
            return builder.RequireAuthorization(p =>
            {
                p.RequireAuthenticatedUser();
            });
        }
    }
}
