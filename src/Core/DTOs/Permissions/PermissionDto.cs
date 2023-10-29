namespace Core.DTOs.Permissions
{
    public class PermissionDto
    {
        public Guid? Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid InterfaceId { get; set; }
        public Guid RoleId { get; set; }
        public string Name { get; set; } = null!;
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public virtual string? RoleName { get; set; }
        public virtual string? InterfaceName { get; set; }
        public virtual string? AreaName { get; set; }
    }
}
