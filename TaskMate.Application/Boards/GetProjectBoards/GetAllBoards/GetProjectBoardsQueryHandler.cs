using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Exceptions;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;

namespace TaskMate.Application.Boards.GetProjectBoards.GetAllBoards
{
    public class GetProjectBoardsQueryHandler(IBoardRepository _boardRepo, IProjectRepository _ProjectRepo, IUserService _userService) : IRequestHandler<GetProjectBoardsQuery, IEnumerable<BoardDto>>
    {
        public async Task<IEnumerable<BoardDto>> Handle(GetProjectBoardsQuery request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            var project = await _ProjectRepo.GetByIdAsync(request.ProjectId);

            if (project is null) throw new NotFoundException($"Project with id {request.ProjectId} does not exist");

            if (project.UserId != userId) throw new ForbiddenException("you don't have access to this board");

            var boards = (await _boardRepo.GetAllAsync(b => b.ProjectId == project.Id))
                .Select(board => new BoardDto
                {
                    Id = board.Id,
                    Name = board.Name,
                    Description = board.Description,
                    CreatedAt = board.CreatedAt,
                });

            return boards;
        }
    }
}
