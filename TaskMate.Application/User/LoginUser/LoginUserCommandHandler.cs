using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Interfaces;

namespace TaskMate.Application.User.LoginUser
{
    internal sealed class LoginUserCommandHandler(IAuthService authService)
        : IRequestHandler<LoginUserCommand, Result<AuthResult>>
    {
        public async Task<Result<AuthResult>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
            return result;
        }
    }
}
