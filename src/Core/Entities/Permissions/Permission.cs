using Core.Entities.Authentication;
using Core.Entities.Bases;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Permissions
{
    public class Permission : BaseTenantEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public Guid InterfaceId { get; set; }
        public Guid RoleId { get; set; }
        public bool CanCreate { get; set; }
        public bool CanRead { get; set; }
        public bool CanUpdate { get; set; }
        public bool CanDelete { get; set; }
        public virtual Interface Interface { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}