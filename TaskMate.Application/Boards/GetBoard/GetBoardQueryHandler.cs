using Mapster;
using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Exceptions;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;

namespace TaskMate.Application.Boards.GetBoard
{
    public class GetBoardQueryHandler(IBoardRepository _boardRepo, IProjectRepository _ProjectRepo, IUserService _userService) : IRequestHandler<GetBoardQuery, BoardDto>
    {
        public async Task<BoardDto> Handle(GetBoardQuery request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();
            // we retreive the board
            var board = await _boardRepo.GetByIdAsync(request.Id);

            if (board is null) throw new NotFoundException($"Board with id {request.Id} does not exist");

            // we have 2 scenarios => 1. board belongs to project / 2. board belongs to user (independent)
            if (board.ProjectId.HasValue)
            {
                // this means we are in case 1 / we need to get the project to check if the current user has access to it or not
                var project = await _ProjectRepo.GetByIdAsync(board.ProjectId.Value);

                if (project is null) throw new NotFoundException($"Project with id {board.ProjectId.Value} does not exist");

                if (project.UserId != userId) throw new ForbiddenException("you don't have access to this board");

                return board.Adapt<BoardDto>();
            }
            else
            {
                // this means we are in case 2
                if (board.UserId != userId) throw new ForbiddenException("you don't have access to this board");
                return board.Adapt<BoardDto>();
            }
        }
    }
}
