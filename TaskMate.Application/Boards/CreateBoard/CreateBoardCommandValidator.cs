using FluentValidation;

namespace TaskMate.Application.Boards.CreateBoard
{
    public class CreateBoardCommandValidator : AbstractValidator<CreateBoardCommand>
    {
        public CreateBoardCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
