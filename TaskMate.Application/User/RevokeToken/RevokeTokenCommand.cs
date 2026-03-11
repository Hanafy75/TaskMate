using MediatR;

namespace TaskMate.Application.User.RevokeToken
{
    public class RevokeTokenCommand : IRequest<Result>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
