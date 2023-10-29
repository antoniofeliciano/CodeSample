namespace Core.DTOs.Assessment
{
    public class AssessmentQueryDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string QueryString { get; set; } = null!;
        public bool IsMultipleDatabase { get; set; }
        public int? ExecutionOrder { get; set; }
        public bool IsActive { get; set; }
        public bool RenderGenericView { get; set; }
        public Guid DatabaseTypeId { get; set; }
    }
}
