using MediatR;

namespace TaskMate.Application.Boards.DeleteBoard
{
    public class DeleteBoardCommand : IRequest
    {
        public int Id { get; set; }
    }
}
