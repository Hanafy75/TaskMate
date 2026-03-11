using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;

namespace TaskMate.Application.Boards.GetProjectBoards.GetAllBoards
{
    internal sealed class GetProjectBoardsQueryHandler(
        IBoardRepository boardRepo,
        IProjectRepository projectRepo,
        IUserService userService)
        : IRequestHandler<GetProjectBoardsQuery, Result<IEnumerable<BoardDto>>>
    {
        public async Task<Result<IEnumerable<BoardDto>>> Handle(GetProjectBoardsQuery request, CancellationToken cancellationToken)
        {
            var userId = userService.GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Error.Unauthorized("Auth.Unauthorized", "Authentication is required.");

            var project = await projectRepo.GetByIdAsync(request.ProjectId, cancellationToken);

            if (project is null)
                return Error.NotFound("Project.NotFound", $"Project with id {request.ProjectId} was not found.");

            if (project.UserId != userId)
                return Error.Forbidden("Board.Forbidden", "You don't have access to this project boards.");

            var boards = (await boardRepo.GetAllAsync(b => b.ProjectId == project.Id, cancellationToken))
                .Select(board => new BoardDto
                {
                    Id = board.Id,
                    Name = board.Name,
                    Description = board.Description,
                    CreatedAt = board.CreatedAt,
                });

            return Result<IEnumerable<BoardDto>>.Ok(boards);
        }
    }
}
