namespace TaskMate.Domain.Entities
{
    public sealed class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public ICollection<Board> Boards { get; set; } = new List<Board>();
    }
}
