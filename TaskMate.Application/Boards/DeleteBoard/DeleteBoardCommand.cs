using MediatR;

namespace TaskMate.Application.Boards.DeleteBoard
{
    public class DeleteBoardCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}
