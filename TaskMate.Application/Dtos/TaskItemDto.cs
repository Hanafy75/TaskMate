using TaskMate.Domain.Enums;

namespace TaskMate.Application.Dtos
{
    public class TaskItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskState Status { get; set; }
    }
}
