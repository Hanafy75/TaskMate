using MediatR;
using TaskMate.Application.Dtos;

namespace TaskMate.Application.User.CreateUser
{
    public class CreateUserCommand : IRequest<AuthResult>
    {
        //command params
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
