using ApplicationServices.Bases;
using Core.DTOs.Authentication;
using Core.DTOs.Configurations;
using Core.Entities.Authentication;
using Core.Entities.Permissions;
using Core.Helpers;
using Core.Interfaces.Repositories.Bases;
using Core.Interfaces.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApplicationServices.Authentication
{
    public class TokenService : BaseTenantService, ITokenService
    {
        private readonly TokenSettings _tokenSettings;
        private readonly IBaseTenantEntityRepository<User> _userRepository;
        public TokenService(TokenSettings tokenSettings,
            IBaseTenantEntityRepository<User> userRepository, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _tokenSettings = tokenSettings;
            _userRepository = userRepository.SetTenantId(TenantId);
        }

        public static Claim PermissionToClaim(Permission permission)
        {
            var value = new StringBuilder();

            if (permission.CanCreate && permission.CanRead && permission.CanUpdate && permission.CanDelete) return new Claim(permission.Interface.SystemName.ToKebabCase(), "a");

            if (permission.CanCreate) value.Append('c');
            if (permission.CanRead) value.Append('r');
            if (permission.CanUpdate) value.Append('u');
            if (permission.CanDelete) value.Append('d');

            return new Claim(permission.Interface.SystemName.ToKebabCase(), value.ToString());
        }

        public async Task<TokenDto> GenerateTokenAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secret = Encoding.ASCII.GetBytes(AuthenticationSettings.AuthenticationSecret);

            var baseClaims = new List<Claim>
                {
                    new Claim("userId",user.Id.ToString()),
                    new Claim("username",user.Username),
                    new Claim("userEmail",user.Email),
                    new Claim("tenantId",user.TenantId.ToString()),
                    new Claim("tenantName",user.Tenant!.Name),
                    new Claim(ClaimTypes.Role,user.Role.Name),
                    new Claim("roleId",user.Role.Id.ToString())
                };

            user.Role.Permissions!.ToList().ForEach((permission) => baseClaims.Add(PermissionToClaim(permission)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(baseClaims),
                Issuer = _tokenSettings.Iss,
                Audience = "Netbrokers.Brazabot",
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)

            };

            var tokenDto = new TokenDto
            {
                AccessToken = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)),
                ExpiresIn = tokenDescriptor.Expires,
                RefreshToken = GenerateRefreshToken(user.Email),
                RefreshExpiresIn = DateTime.UtcNow.AddMinutes(65)
            };

            user.LastSignIn = DateTime.Now;
            user.RefreshToken = tokenDto.RefreshToken;
            await _userRepository.Update(user);

            return tokenDto;
        }

        public async Task<ClaimsIdentity> GetClaimsIdentityFromExpiredToken(string token)
        {
            var secret = Encoding.ASCII.GetBytes(AuthenticationSettings.AuthenticationSecret);
            var tokenHandler = new JwtSecurityTokenHandler();
            var parameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secret),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = _tokenSettings.Iss,
                ValidAudience = "Netbrokers.Brazabot",
                ValidateLifetime = false,
            };
            var tokenResult = await tokenHandler.ValidateTokenAsync(token, parameters);
            return tokenResult.ClaimsIdentity;
        }

        private static string GenerateRefreshToken(string userEmail)
        {
            var key = Base64Helper.EncodeToBase64($"{userEmail}4f1nc.");
            var refreshToken = Base64Helper.EncodeToBase64($"{DateTime.UtcNow}{key}{Guid.NewGuid}4f1nc.");
            return refreshToken;
        }
    }
}
