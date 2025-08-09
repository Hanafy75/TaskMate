using TaskMate.Application.Dtos;
using TaskMate.Application.User.CreateUser;

namespace TaskMate.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResult> RegisterAsync(CreateUserCommand command);
        Task<AuthResult> GetTokenAsync(string email, string password);
        Task<AuthResult> GetRefreshTokenAsync(string refreshToken);

        Task<bool> RevokeRefreshTokenAsync(string refreshToken);
    }
}
