using Mapster;
using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;

namespace TaskMate.Application.Boards.GetBoard
{
    internal sealed class GetBoardQueryHandler(
        IBoardRepository boardRepo,
        IProjectRepository projectRepo,
        IUserService userService)
        : IRequestHandler<GetBoardQuery, Result<BoardDto>>
    {
        public async Task<Result<BoardDto>> Handle(GetBoardQuery request, CancellationToken cancellationToken)
        {
            var userId = userService.GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Error.Unauthorized("Auth.Unauthorized", "Authentication is required.");

            // we retreive the board
            var board = await boardRepo.GetByIdAsync(request.Id, cancellationToken);

            if (board is null)
                return Error.NotFound("Board.NotFound", $"Board with id {request.Id} was not found.");

            // we have 2 scenarios => 1. board belongs to project / 2. board belongs to user (independent)
            if (board.ProjectId.HasValue)
            {
                // this means we are in case 1 / we need to get the project to check if the current user has access to it or not
                var project = await projectRepo.GetByIdAsync(board.ProjectId.Value, cancellationToken);

                if (project is null)
                    return Error.NotFound("Project.NotFound", $"Project with id {board.ProjectId.Value} was not found.");

                if (project.UserId != userId)
                    return Error.Forbidden("Board.Forbidden", "You don't have access to this board.");

                return board.Adapt<BoardDto>();
            }
            else
            {
                // this means we are in case 2
                if (board.UserId != userId)
                    return Error.Forbidden("Board.Forbidden", "You don't have access to this board.");

                return board.Adapt<BoardDto>();
            }
        }
    }
}
