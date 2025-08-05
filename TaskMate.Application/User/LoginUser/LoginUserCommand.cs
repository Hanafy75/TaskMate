using MediatR;
using TaskMate.Application.Dtos;

namespace TaskMate.Application.User.LoginUser
{
    public class LoginUserCommand : IRequest<AuthResult>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set;} = null!;
    }
}
