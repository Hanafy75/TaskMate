namespace TaskMate.Application.Options
{
    public class JWTOptions
    {
        public string Key { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int Duration { get; set; }
    }

}
