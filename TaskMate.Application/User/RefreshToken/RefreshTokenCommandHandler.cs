using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Exceptions;
using TaskMate.Application.Interfaces;

namespace TaskMate.Application.User.RefreshToken
{
    public class RefreshTokenCommandHandler(IAuthService _authService) : IRequestHandler<RefreshTokenCommand, AuthResult>
    {
        public async Task<AuthResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.RefreshToken)) throw new SecurityTokenException("No refresh Token Provided");

            var result = await _authService.GetRefreshTokenAsync(request.RefreshToken);
            return result;
        }
    }
}
