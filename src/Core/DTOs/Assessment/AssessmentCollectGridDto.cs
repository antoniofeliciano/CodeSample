using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs.Assessment
{
    public class AssessmentCollectGridDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
        public string ClientName { get; set; } = null!;
        public string Details { get; set; } = null!;
        public DateTimeOffset CollectDate { get; set; }
        public string FileName { get; set; } = null!;
        public string TechnicalResponsible { get; set; } = null!;
    }
}
