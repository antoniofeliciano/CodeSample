using Core.Entities.Bases;
using Core.Entities.Permissions;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Authentication
{
    public class Role : BaseTenantEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [MaxLength(100)]
        public string Description { get; set; } = null!;
        public virtual IEnumerable<Permission>? Permissions { get; set; }
    }
}
