namespace Core.DTOs.Authentication
{
    public class SystemApiKeyDto
    {
        public Guid? Id { get; set; }
        public Guid TenantId { get; set; }
        public string ApiKey { get; set; } = null!;
        public string ApiSecret { get; set; } = null!;
        public string AppName { get; set; } = null!;
        public DateTimeOffset? ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public bool InfiniteExpirationDate { get; set; }
    }
}
