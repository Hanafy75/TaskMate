using MediatR;
using TaskMate.Application.Dtos;

namespace TaskMate.Application.Boards.GetProjectBoards.GetAllBoards
{
    public class GetProjectBoardsQuery : IRequest<IEnumerable<BoardDto>>
    {
        public int ProjectId { get; set; }
    }
}
