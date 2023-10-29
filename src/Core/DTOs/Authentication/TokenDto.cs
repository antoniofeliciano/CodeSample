namespace Core.DTOs.Authentication
{
    public class TokenDto
    {
        public string AccessToken { get; set; } = null!;
        public DateTimeOffset? ExpiresIn { get; set; }
        public string RefreshToken { get; set; } = null!;
        public DateTimeOffset RefreshExpiresIn { get; set; }
    }
}
