using FluentValidation;

namespace TaskMate.Application.Boards.UpdateBoard
{
    public class UpdateBoardCommandValidator : AbstractValidator<UpdateBoardCommand>
    {
        public UpdateBoardCommandValidator()
        {
            RuleFor(x=>x.Id).NotEmpty();

            RuleFor(x=>x.Name).NotEmpty();

        }
    }
}
