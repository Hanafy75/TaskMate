using TaskMate.Application.Dtos;
using TaskMate.Application.User.CreateUser;

namespace TaskMate.Application.Interfaces
{
    public interface IAuthService
    {
        Task<Result<AuthResult>> RegisterAsync(CreateUserCommand command, CancellationToken cancellationToken = default);
        Task<Result<AuthResult>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<Result<AuthResult>> GetRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);

        Task<Result> RevokeRefreshTokenAsync(string refreshToken, CancellationToken cancellationToken = default);
    }
}
