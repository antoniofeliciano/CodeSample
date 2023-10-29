using Core.Entities.Bases;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Tenants
{
    public class Tenant : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; } = null!;
        public string? Logo { get; set; }
        public string? SmallLogo { get; set; }
    }
}