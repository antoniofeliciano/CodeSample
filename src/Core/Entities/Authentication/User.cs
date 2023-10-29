using Core.Entities.Bases;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Authentication
{
    public class User : BaseTenantEntity
    {
        [MaxLength(100)]
        public string Username { get; set; } = null!;

        [MaxLength(100)]
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Salt { get; set; } = null!;
        public Guid RoleId { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset? LastSignIn { get; set; }
        public string? RefreshToken { get; set; }
        public string? ProfilePicture { get; set; }
        public virtual Role Role { get; set; } = null!;

    }
}
