using FluentValidation;

namespace TaskMate.Application.Boards.DeleteBoard
{
    public class DeleteBoardCommandValidator : AbstractValidator<DeleteBoardCommand>
    {
        public DeleteBoardCommandValidator()
        {
            RuleFor(x=>x.Id).NotEmpty();
        }
    }
}
