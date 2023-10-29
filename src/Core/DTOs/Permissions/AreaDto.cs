namespace Core.DTOs.Permissions
{
    public class AreaDto
    {
        public Guid? Id { get; set; }
        public Guid TenantId { get; set; }
        public string? Icon { get; set; }
        public string Name { get; set; } = null!;
        public int Position { get; set; }
        public bool Visible { get; set; }
    }
}