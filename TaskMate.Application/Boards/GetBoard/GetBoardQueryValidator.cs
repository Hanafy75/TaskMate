using FluentValidation;

namespace TaskMate.Application.Boards.GetBoard
{
    public class GetBoardQueryValidator : AbstractValidator<GetBoardQuery>
    {
        public GetBoardQueryValidator()
        {
            RuleFor(x=>x.Id).NotEmpty();
        }
    }
}
