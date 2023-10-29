using Core.Entities.Bases;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Permissions
{
    public class Area : BaseTenantEntity
    {
        [MaxLength(50)]
        public string? Icon { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public int Position { get; set; }
        public bool Visible { get; set; }
    }
}