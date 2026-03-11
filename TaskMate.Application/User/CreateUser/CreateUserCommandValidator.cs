using FluentValidation;

namespace TaskMate.Application.User.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(20);


            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(20);


            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(50);


            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}
