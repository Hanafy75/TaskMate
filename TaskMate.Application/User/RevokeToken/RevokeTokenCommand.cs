using MediatR;

namespace TaskMate.Application.User.RevokeToken
{
    public class RevokeTokenCommand : IRequest<bool>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
