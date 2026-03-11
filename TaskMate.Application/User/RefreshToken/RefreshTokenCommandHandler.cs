using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Interfaces;

namespace TaskMate.Application.User.RefreshToken
{
    internal sealed class RefreshTokenCommandHandler(IAuthService authService)
        : IRequestHandler<RefreshTokenCommand, Result<AuthResult>>
    {
        public async Task<Result<AuthResult>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return Error.Validation(nameof(request.RefreshToken), "No refresh token provided.");

            var result = await authService.GetRefreshTokenAsync(request.RefreshToken, cancellationToken);
            return result;
        }
    }
}
