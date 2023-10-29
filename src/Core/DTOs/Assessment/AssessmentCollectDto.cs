using Microsoft.AspNetCore.Http;

namespace Core.DTOs.Assessment
{
    public class AssessmentCollectDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
        public string ClientName { get; set; } = null!;
        public string TechnicalResponsible { get; set; } = null!;
        public string Details { get; set; } = null!;
        public DateTimeOffset CollectDate { get; set; }
        public string CollectResult { get; set; } = null!;
        public string FileName { get; set; } = null!;

    }
}
