using ApplicationServices.Bases;
using Core.DTOs.Authentication;
using Core.DTOs.Permissions;
using Core.Entities.Authentication;
using Core.Interfaces.Repositories.Bases;
using Core.Interfaces.Services.Authentication;
using Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;

namespace ApplicationServices.Authentication
{
    public class AuthenticationService : BaseTenantService, IAuthenticationService
    {
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IBaseTenantEntityRepository<User> _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IValidator<UserDto> _userValidator;
        private readonly IValidator<CreateUserDto> _createUserValidator;
        private readonly IValidator<LoginDto> _loginValidator;

        const int keySize = 64;
        const int iterations = 350000;

        public AuthenticationService(ILogger<AuthenticationService> logger,
            IMemoryCache memoryCache,
            IBaseTenantEntityRepository<User> userRepository,
            ITokenService tokenService,
            IValidator<UserDto> userValidator,
            IValidator<CreateUserDto> createUserValidator,
            IValidator<LoginDto> loginValidator,
            IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _userRepository = userRepository.SetTenantId(TenantId);
            _tokenService = tokenService;
            _userValidator = userValidator;
            _createUserValidator = createUserValidator;
            _loginValidator = loginValidator;

        }

        public async Task<Result<TokenDto>> SignInAsync(LoginDto login)
        {
            try
            {
                var validationResult = await _loginValidator.ValidateAsync(login);
                if (!validationResult.IsValid) return new ErrorResult<TokenDto>(validationResult.Errors.Select(e => e.ErrorMessage));

                var foudUser = await _memoryCache.GetOrCreateAsync(login.Email, async (e) =>
                {
                    e.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(20);
                    return await _userRepository.GetDbSet()
                    .Where(u => u.Email == login.Email && u.IsActive)
                    .Include(r => r.Role)
                    .ThenInclude(p => p.Permissions!)
                    .ThenInclude(i => i.Interface)
                    .ThenInclude(m => m.Area)
                    .Include(t => t.Tenant)
                    .FirstOrDefaultAsync();

                });

                if (foudUser is null || foudUser.Password != GeneratePasswordHashAsync(login.Password, foudUser.Salt))
                    return new NotFoundResult<TokenDto>(new[] { "Incorrect user or password " });

                var token = await _tokenService.GenerateTokenAsync(foudUser);
                return new SuccessResult<TokenDto>(token);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail getting user informations.");
                throw;
            }
        }
        private static string GeneratePasswordHashAsync(string password, string salt)
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt), iterations, HashAlgorithmName.SHA512, keySize);
            return Convert.ToHexString(hash);
        }
        public async Task<Result<TokenDto>> RefreshTokenAsync(RefreshTokenDto refreshTokenDto)
        {
            var identity = await _tokenService.GetClaimsIdentityFromExpiredToken(refreshTokenDto.AccessToken);
            if (identity is null) return new ErrorResult<TokenDto>("Invalid token.");

            var email = identity.FindFirst("userEmail")?.Value;

            var foudUser = await _memoryCache.GetOrCreateAsync(email!, async (e) =>
            {
                e.AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(20);
                return await _userRepository.GetDbSet()
                .Where(u => u.Email == email && u.IsActive)
                .Include(r => r.Role)
                .ThenInclude(p => p.Permissions!)
                .ThenInclude(i => i.Interface)
                .ThenInclude(m => m.Area)
                .Include(t => t.Tenant)
                .FirstOrDefaultAsync();

            });



            if (foudUser?.RefreshToken is null || foudUser?.RefreshToken != refreshTokenDto.RefreshToken) return new ErrorResult<TokenDto>("Invalid token.");

            var token = await _tokenService.GenerateTokenAsync(foudUser);
            return new SuccessResult<TokenDto>(token);
        }
        public async Task<Result> CreateUserAsync(CreateUserDto user)
        {
            try
            {
                var validationResult = await _createUserValidator.ValidateAsync(user);
                if (!validationResult.IsValid) return new ErrorResult<object>(validationResult.Errors.Select(e => e.ErrorMessage));
                var salt = Convert.ToHexString(RandomNumberGenerator.GetBytes(keySize));

                var newUser = await _userRepository.SetTenantId(user.TenantId).Insert(new User
                {
                    Username = user.Username,
                    Email = user.Email,
                    IsActive = user.IsActive,
                    RoleId = user.RoleId,
                    ProfilePicture = user.ProfilePicture,
                    Password = GeneratePasswordHashAsync(user.Password, salt),
                    Salt = salt

                });
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fail adding user");
                throw;
            }
        }
        public async Task<TokenDto> InternalSignInAsync(Guid userId)
        {
            var foudUser = await _userRepository.GetByIdAsync(userId,
                    include: u => u.Include(r => r.Role)
                .ThenInclude(p => p.Permissions!)
                .ThenInclude(i => i.Interface)
                .ThenInclude(m => m.Area!));

            return await _tokenService.GenerateTokenAsync(foudUser!);
        }

    }
}
