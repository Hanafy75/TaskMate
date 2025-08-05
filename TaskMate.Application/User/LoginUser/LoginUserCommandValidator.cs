using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TaskMate.Domain.Entities;

namespace TaskMate.Application.User.LoginUser
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator(UserManager<ApplicationUser> userManager)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .MaximumLength(50);

            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(50);
        }
    }
}
