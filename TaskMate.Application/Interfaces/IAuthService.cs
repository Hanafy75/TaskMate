using TaskMate.Application.Dtos;
using TaskMate.Application.User.CreateUser;

namespace TaskMate.Application.Interfaces
{
    internal interface IAuthService
    {
        Task<AuthResult> RegisterAsync(CreateUserCommand command);
        Task<AuthResult> GetTokenAsync(string email, string name);
        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
    }
}
