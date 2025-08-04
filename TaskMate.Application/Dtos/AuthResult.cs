namespace TaskMate.Application.Dtos
{
    public class AuthResult
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
