using MediatR;
using TaskMate.Application.Interfaces;

namespace TaskMate.Application.User.RevokeToken
{
    public class RevokeTokenCommandHandler(IAuthService authService) : IRequestHandler<RevokeTokenCommand, bool>
    {
        private readonly IAuthService _authService = authService;

        public async Task<bool> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
        {
            var result = await _authService.RevokeRefreshTokenAsync(request.RefreshToken);
            return result;
        }
    }
}
