using MediatR;
using TaskMate.Application.Dtos;

namespace TaskMate.Application.Boards.CreateBoard
{
    public class CreateBoardCommand : IRequest<int>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int? ProjectId { get; set; }
    }
}
