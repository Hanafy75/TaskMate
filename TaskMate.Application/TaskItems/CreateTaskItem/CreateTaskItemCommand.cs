using MediatR;

namespace TaskMate.Application.TaskItems.CreateTaskItem
{
    public class CreateTaskItemCommand : IRequest<int>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }
        public int BoardId { get; set; }
    }
}
