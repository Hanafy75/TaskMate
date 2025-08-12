using MediatR;

namespace TaskMate.Application.Boards.UpdateBoard
{
    public class UpdateBoardCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
