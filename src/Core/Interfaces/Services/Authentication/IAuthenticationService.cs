using Core.DTOs.Authentication;
using Core.DTOs.Permissions;
using Core.Results;

namespace Core.Interfaces.Services.Authentication
{
    public interface IAuthenticationService
    {
        Task<Result<TokenDto>> SignInAsync(LoginDto login);
        Task<TokenDto> InternalSignInAsync(Guid userId);
        Task<Result> CreateUserAsync(CreateUserDto user);
        Task<Result<TokenDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto);
    }
}
