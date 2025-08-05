using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Interfaces;

namespace TaskMate.Application.User.LoginUser
{
    internal class LoginUserCommandHandler(IAuthService authService) : IRequestHandler<LoginUserCommand, AuthResult>
    {
        private readonly IAuthService _authService = authService;

        public async Task<AuthResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var result = await _authService.GetTokenAsync(request.Email, request.Password);
            return result;
        }
    }
}
