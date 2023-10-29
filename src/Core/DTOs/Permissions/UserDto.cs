using Core.Entities.Authentication;

namespace Core.DTOs.Permissions
{
    public class UserDto
    {
        public Guid? Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid RoleId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public string? ProfilePicture { get; set; }
        public Role Role { get; set; } = null!;
        public bool IsActive { get; set; }
    }
}
