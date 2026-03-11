using MediatR;
using TaskMate.Application.Dtos;

namespace TaskMate.Application.Boards.GetBoard
{
    public class GetBoardQuery : IRequest<Result<BoardDto>>
    {
        public int Id { get; set; }
    }
}
