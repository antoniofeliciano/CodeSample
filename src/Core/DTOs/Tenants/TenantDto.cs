namespace Core.DTOs.Tenants
{
    public class TenantDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Logo { get; set; }
        public string? SmallLogo { get; set; }
        public object? Menu { get; set; }
    }
}
