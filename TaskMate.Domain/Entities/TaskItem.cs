using TaskMate.Domain.Enums;

namespace TaskMate.Domain.Entities
{
    public sealed class TaskItem
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        public TaskState Status { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; } = null!;
    }
}