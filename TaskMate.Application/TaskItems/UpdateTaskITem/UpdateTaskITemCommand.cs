using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Domain.Enums;

namespace TaskMate.Application.TaskItems.UpdateTaskITem
{
    public class UpdateTaskITemCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskState Status { get; set; }
    }
}
