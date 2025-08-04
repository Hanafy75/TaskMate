using FluentValidation;
using Microsoft.AspNetCore.Identity;
using TaskMate.Domain.Entities;

namespace TaskMate.Application.User.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator(UserManager<ApplicationUser> userManager)
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .MaximumLength(20);


            RuleFor(x => x.LastName)
                .NotEmpty()
                .MaximumLength(20);


            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress()
                .MustAsync(async (email, cancellation) =>
                {
                    var user = await userManager.FindByEmailAsync(email);
                    return user is null;
                }).WithMessage("Email already registered!");


            RuleFor(x => x.Password)
                .NotEmpty()
                .MaximumLength(30);
        }
    }
}
