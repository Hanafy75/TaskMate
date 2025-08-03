namespace TaskMate.Domain.Entities
{
    public sealed class Board
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? ProjectId { get; set; } // nullable coz user maybe want to create a small number of tasks so no need to create a project.
        public Project? Project { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;

        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    }
}