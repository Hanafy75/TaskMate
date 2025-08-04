using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Interfaces;

namespace TaskMate.Application.User.CreateUser
{
    internal class CreateUserCommandHandler(IAuthService authService) : IRequestHandler<CreateUserCommand, AuthResult>
    {
        private readonly IAuthService _authService = authService;

        public async Task<AuthResult> Handle(CreateUserCommand request, CancellationToken cancellationToken = default)
        {
            var result = await _authService.RegisterAsync(request);
            return result;
        }
    }
}
