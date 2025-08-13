using FluentValidation;

namespace TaskMate.Application.Boards.GetProjectBoards.GetAllBoards
{
    public class GetProjectBoardsQueryValidator : AbstractValidator<GetProjectBoardsQuery>
    {
        public GetProjectBoardsQueryValidator()
        {
            RuleFor(x=>x.ProjectId).NotEmpty();
        }
    }
}
