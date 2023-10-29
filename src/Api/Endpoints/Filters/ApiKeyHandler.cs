using Core.Entities.Authentication;
using Core.Interfaces.Repositories.Bases;
using Infrastructure.Data.Repositories.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace Api.Endpoints.Filters
{
    public class ApiKeyHandler : AuthorizationHandler<ApiKeyAuthentication>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IBaseTenantEntityRepository<SystemApiKey> _systemApiKeyRepository;
        private readonly IMemoryCache _memoryCache;
        public ApiKeyHandler(IHttpContextAccessor httpContextAccessor,
            IBaseTenantEntityRepository<SystemApiKey> systemApiKeyRepository,
            IMemoryCache memoryCache)
        {
            _httpContextAccessor = httpContextAccessor;
            _systemApiKeyRepository = systemApiKeyRepository;
            _memoryCache = memoryCache;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ApiKeyAuthentication requirement)
        {
            if (_httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("X-API-Key", out var receivedApiKey)
             && _httpContextAccessor.HttpContext!.Request.Headers.TryGetValue("X-API-Secret", out var receivedApiSecret))
            {

                var key = await _memoryCache.GetOrCreateAsync(receivedApiKey, async i =>
                {
                    var dt = await _systemApiKeyRepository.GetDbSet().AsNoTracking().FirstOrDefaultAsync(a =>
                    a.IsActive
                    && (a.ExpirationDate <= DateTime.Now || a.InfiniteExpirationDate )
                    && (a.ApiKey == receivedApiKey.ToString() && a.ApiSecret == receivedApiSecret.ToString()));

                    return dt;
                });

                if (key != null)
                {
                    var claims = new[]
                    {
                        new Claim(ClaimTypes.Name,key.AppName),
                        new Claim("tenantId", key.TenantId.ToString()),
                    };

                    var identity = new ClaimsIdentity(claims, "ApiKey");

                    _httpContextAccessor.HttpContext.User.AddIdentity(identity);

                    context.Succeed(requirement);
                    return;
                }
            }

            context.Fail(new AuthorizationFailureReason(this, "invalid api key"));
            return;
        }
    }
}
