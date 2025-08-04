using System.Security.Claims;
using TaskMate.Domain.Entities;

namespace TaskMate.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateJwtToken(ApplicationUser user, IList<Claim> userClaims);
        public string GenerateRefreshToken();
    }
}
