using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;

namespace TaskMate.Application.Home.GetAllBoards
{
    internal sealed class GetAllBoardsQueryHandler(
        IBoardRepository boardRepo,
        IUserService userService)
        : IRequestHandler<GetAllBoardsQuery, Result<IEnumerable<BoardDto>>>
    {
        public async Task<Result<IEnumerable<BoardDto>>> Handle(GetAllBoardsQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = userService.GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(currentUserId))
                return Error.Unauthorized("Auth.Unauthorized", "User must be logged in.");

            var boards = await boardRepo.GetIndependentBoardDtoAsync(currentUserId, cancellationToken);
            return Result<IEnumerable<BoardDto>>.Ok(boards);
        }
    }
}
