using MediatR;
using TaskMate.Application.Interfaces;

namespace TaskMate.Application.User.RevokeToken
{
    internal sealed class RevokeTokenCommandHandler(IAuthService authService)
        : IRequestHandler<RevokeTokenCommand, Result>
    {
        public async Task<Result> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await authService.RevokeRefreshTokenAsync(request.RefreshToken, cancellationToken);
            return result;
        }
    }
}
