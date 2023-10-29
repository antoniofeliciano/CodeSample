using Core.DTOs.Authentication;
using Core.Entities.Authentication;
using System.Security.Claims;

namespace Core.Interfaces.Services.Authentication
{
    public interface ITokenService
    {
        Task<TokenDto> GenerateTokenAsync(User user);
        Task<ClaimsIdentity> GetClaimsIdentityFromExpiredToken(string token);

    }
}
