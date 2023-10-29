namespace Core.DTOs.Authentication
{
    public class RefreshTokenDto
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
