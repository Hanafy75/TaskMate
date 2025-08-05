using System.Text.Json.Serialization;

namespace TaskMate.Application.Dtos
{
    public class AuthResult
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public bool IsAuthenticated{ get; set; }
        public DateTime ExpiresOn { get; set; }

        [JsonIgnore]
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiresOn { get; set; }

    }
}
