using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Interfaces;

namespace TaskMate.Application.User.CreateUser
{
    internal sealed class CreateUserCommandHandler(IAuthService authService)
        : IRequestHandler<CreateUserCommand, Result<AuthResult>>
    {
        public async Task<Result<AuthResult>> Handle(CreateUserCommand request, CancellationToken cancellationToken = default)
        {
            var result = await authService.RegisterAsync(request, cancellationToken);
            return result;
        }
    }
}
