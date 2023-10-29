using Core.Entities.Bases;

namespace Core.Entities.Authentication
{
    public class SystemApiKey : BaseTenantEntity
    {
        public string ApiKey { get; set; } = null!; 
        public string ApiSecret { get; set; } = null!;
        public string AppName { get; set; } = null!;
        public DateTimeOffset? ExpirationDate { get; set; }
        public bool IsActive { get; set; }
        public bool InfiniteExpirationDate { get; set; }
    }
}