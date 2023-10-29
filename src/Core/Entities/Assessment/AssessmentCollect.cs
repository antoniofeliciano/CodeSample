using Core.Entities.Bases;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Assessment
{
    public class AssessmentCollect : BaseTenantEntity
    {
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        [MaxLength(50)]
        public string ClientName { get; set; } = null!;
        public DateTimeOffset CollectDate { get; set; }

        [MaxLength(300)]
        public string Details { get; set; } = null!;

        [MaxLength(50)]
        public string FileName { get; set; } = null!;
        public byte[] CollectResult { get; set; } = null!;
        [MaxLength(50)]
        public string TechnicalResponsible { get; set; } = null!;
    }
}