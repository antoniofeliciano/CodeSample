using ApplicationServices.Bases;
using AutoMapper;
using Core.DTOs.Permissions;
using Core.Entities.Authentication;
using Core.Interfaces.Repositories.Bases;
using Core.Interfaces.Services.Permissions;
using Core.Results;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Dynamic.Core;

namespace ApplicationServices.Permissions
{
    public class UserService : BaseTenantService, IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IBaseTenantEntityRepository<User> _userRepository;
        private readonly IValidator<CreateUserDto> _createUserDtoValidator;
        private readonly IValidator<EditUserDto> _editUserDtoValidator;
        private readonly IMapper _mapper;
        public UserService(
            ILogger<UserService> logger,
            IBaseTenantEntityRepository<User> UserRepository,
            IValidator<CreateUserDto> createUserDtoValidator,
            IValidator<EditUserDto> editUserDtoValidator,
            IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
            _userRepository = UserRepository;
            _createUserDtoValidator = createUserDtoValidator;
            _editUserDtoValidator = editUserDtoValidator;
            _mapper = mapper;
        }
        public async Task<Result> EditUserAsync(EditUserDto userDto)
        {
            try
            {
                var validationResult = await _editUserDtoValidator.ValidateAsync(userDto);
                if (!validationResult.IsValid) return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage));

                var user = await _userRepository.SetTenantId(userDto.TenantId).GetByIdAsync(userDto.Id);
                if (user == null) return new NotFoundResult(new[] { "Invalid user." });

                user.ProfilePicture = userDto.ProfilePicture;
                user.Username = userDto.Username;
                user.Email = userDto.Email;
                if (!userDto.Password.IsNullOrEmpty())
                {
                    user.Password = userDto.Password!;
                }
                user.IsActive = userDto.IsActive;

                await _userRepository.Update(user);
                return new CreatedResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing User.");
                throw;
            }
        }
        public async Task<Result> DeleteUserAsync(Guid tenantId, Guid UserId)
        {
            try
            {
                if (!(await _userRepository.SetTenantId(tenantId).ExistsNotRemovedAsync(s => s.Id == UserId))) return new ErrorResult("User not found.");
                await _userRepository.SetTenantId(tenantId).Delete(UserId);
                return new Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail editing User.");
                throw;
            }
        }
        public async Task<Result<IEnumerable<UserGridDto>>> GetUserForGridAsync(Guid tenantId, string? searchTerm, int? pageNumber, int? pageSize, string? orderBy, string? direction)
        {
            try
            {
                var query = _userRepository.SetTenantId(tenantId).GetTenantDbSet()
                    .Include(r => r.Role)
                    .AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    searchTerm = searchTerm.ToLower();
                    query = query.Where(r => r.Username.ToLower().Contains(searchTerm.ToLower()));
                }

                if (!string.IsNullOrEmpty(orderBy))
                {
                    query = query.OrderBy($"{orderBy} {(direction == "desc" ? "descending" : "ascending")}");
                }
                else
                {
                    query = query.OrderBy($"Username {(direction == "desc" ? "descending" : "ascending")}");
                }

                if (pageNumber.HasValue && pageSize.HasValue)
                {
                    query = query.Skip((pageNumber.Value - 1) * pageSize.Value)
                    .Take(pageSize.Value);
                }

                return new SuccessResult<IEnumerable<UserGridDto>>(_mapper.Map<IEnumerable<UserGridDto>>(await query.ToListAsync()));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail getting Users.");
                throw;
            }
        }

        public async Task<Result<string>> GetUserProfilePicAsync()
        {
            try
            {
                var user = await _userRepository.SetTenantId(TenantId).GetByIdAsync(UserId);
                if (user is null) return new NotFoundResult<string>();
                return new SuccessResult<string>(user.ProfilePicture);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "fail getting Users.");
                throw;
            }
        }
    }
}
