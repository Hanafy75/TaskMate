using System.Security.Claims;
using TaskMate.Application.Interfaces;

namespace TaskMate.WebAPI.Services
{
    public class UserService(IHttpContextAccessor _context) : IUserService
    {
        public string? GetCurrentUserId()
        {
            return _context.HttpContext!.User.FindFirstValue("uid");
        }
    }
}
