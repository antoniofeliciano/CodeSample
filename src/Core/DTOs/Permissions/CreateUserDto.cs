namespace Core.DTOs.Permissions
{
    public class CreateUserDto
    {
        public Guid? Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid RoleId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? ProfilePicture { get; set; }
        public bool IsActive { get; set; }
    }
}
