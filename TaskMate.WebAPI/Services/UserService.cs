using System.Security.Claims;
using TaskMate.Application.Interfaces;

namespace TaskMate.WebAPI.Services
{
    public class UserService(IHttpContextAccessor _context) : IUserService
    {
        public string? GetCurrentUserIdAsync()
        {
            return _context.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
