using Microsoft.AspNetCore.Identity;

namespace TaskMate.Domain.Entities
{
    public sealed class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;

        public ICollection<Project> Projects { get; set; } = new List<Project>();
        public ICollection<Board>? Boards { get; set; }

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}