using MediatR;
using TaskMate.Application.Dtos;

namespace TaskMate.Application.Home.GetAllBoards
{
    public class GetAllBoardsQuery : IRequest<IEnumerable<BoardDto>>
    {
    }
}
