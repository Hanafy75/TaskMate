using MediatR;
using TaskMate.Application.Dtos;

namespace TaskMate.Application.User.RefreshToken
{
    public class RefreshTokenCommand :IRequest<AuthResult>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
