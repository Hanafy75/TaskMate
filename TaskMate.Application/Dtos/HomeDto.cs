namespace TaskMate.Application.Dtos
{
    public class HomeDto
    {
        public IEnumerable<ProjectDto> Projects { get; set; } = null!;
        public IEnumerable<BoardDto> boards { get; set; } = null!;
    }
}
