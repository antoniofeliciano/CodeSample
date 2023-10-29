using Core.Entities.Bases;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Assessment
{
    public class AssessmentQuery : BaseTenantEntity
    {
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(300)]
        public string Description { get; set; } = null!;
        public string QueryString { get; set; } = null!;
        public bool IsMultipleDatabase { get; set; } 
        public bool IsActive { get; set; }
        public int? ExecutionOrder { get; set; }
        public bool RenderGenericView { get; set; }
        public Guid DatabaseTypeId { get; set; }

        public virtual DatabaseType? DatabaseType { get; set; }
    }
}
