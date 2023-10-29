using Core.DTOs.Permissions;

namespace Core.DTOs.Authentication
{
    public class RoleDto
    {
        public Guid? Id { get; set; }
        public Guid TenantId { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public virtual IEnumerable<PermissionDto>? Permissions { get; set; }
    }
}
