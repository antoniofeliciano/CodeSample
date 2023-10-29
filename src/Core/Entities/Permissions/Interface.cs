using Core.Entities.Bases;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Permissions
{
    public class Interface : BaseTenantEntity
    {
        [MaxLength(50)]
        public string? Icon { get; set; }
        
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        [MaxLength(100)]
        public string SystemName { get; set; } = null!;
        
        [MaxLength(100)]
        public string Route { get; set; } = null!;
        public Guid AreaId { get; set; }
        public bool Visible { get; set; }
        public bool Renderable { get; set; }
        public virtual Area? Area { get; set; }

    }
}