namespace Core.DTOs.Permissions
{
    public class InterfaceGridDto
    {
        public Guid? Id { get; set; }
        public Guid TenantId { get; set; }
        public Guid AreaId { get; set; }
        public string? Icon { get; set; }
        public string Name { get; set; } = null!;
        public string SystemName { get; set; } = null!;
        public string Route { get; set; } = null!;
        public string Area { get; set; } = null!;
        public bool Visible { get; set; }
        public bool Renderable { get; set; }

    }
}