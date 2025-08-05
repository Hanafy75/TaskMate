using FluentValidation;

namespace TaskMate.Application.User.RevokeToken
{
    public class RevokeTokenCommandValidator : AbstractValidator<RevokeTokenCommand>
    {
        public RevokeTokenCommandValidator()
        {
            RuleFor(x=>x.RefreshToken)
                .NotEmpty();
        }
    }
}
