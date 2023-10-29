using Core.Entities.Authentication;
using Core.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace ApplicationServices.Bases
{
    public class BaseTenantService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public readonly Guid TenantId;
        public readonly Guid UserId;
        public readonly Guid RoleId;
        public BaseTenantService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;

            var excludePaths = new List<string>
            {
                "/Authentication/SignIn",
                "/Authentication/RefreshToken",
            };


            if (!_contextAccessor.HttpContext.User.Identities.Any(c => c.IsAuthenticated)) return;

            var isApiKey = _contextAccessor.HttpContext.User.Identities.Any(c => c.AuthenticationType == "ApiKey");


            if (!excludePaths.Contains(_contextAccessor.HttpContext.Request.Path))
            {
                var tenantIdclaim = _contextAccessor.HttpContext.User.FindFirst("tenantId")?.Value.ToString();
                var userIdClaim = _contextAccessor.HttpContext.User.FindFirst("userId")?.Value.ToString();
                var roleIdclaim = _contextAccessor.HttpContext.User.FindFirst("roleId")?.Value.ToString();


                var userNameClaim = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.Name)?.Value.ToString();

                if (string.IsNullOrEmpty(tenantIdclaim) || !Guid.TryParse(tenantIdclaim, out Guid tenantId))
                    throw new InvalidTenantException();
                
                if (!isApiKey)
                {
                    if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out Guid userId))
                        throw new InvalidTenantException();
                    UserId = userId;

                    if (string.IsNullOrEmpty(roleIdclaim) || !Guid.TryParse(roleIdclaim, out Guid roleId))
                        throw new InvalidTenantException();
                    RoleId = roleId;
                }

                TenantId = tenantId;


            }
        }
    }
}