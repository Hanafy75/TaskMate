using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;

namespace TaskMate.Application.Home.GetAllBoards
{
    public class GetAllProjectsQueryHandler(IBoardRepository _boardRepo, IUserService _userService) : IRequestHandler<GetAllBoardsQuery, IEnumerable<BoardDto>>
    {
        public async Task<IEnumerable<BoardDto>> Handle(GetAllBoardsQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _userService.GetCurrentUserId();

            if (currentUserId is null) throw new UnauthorizedAccessException("user must be logged in.");

            var boards = await _boardRepo.GetIndependentBoardDtoAsync(currentUserId);
            return boards;
        }
    }
}
