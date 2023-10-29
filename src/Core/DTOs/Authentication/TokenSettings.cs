namespace Core.DTOs.Authentication
{
    public class TokenSettings
    {
        public string Iss { get; set; } = null!;
        public string Aud { get; set; } = null!;
    }
}
