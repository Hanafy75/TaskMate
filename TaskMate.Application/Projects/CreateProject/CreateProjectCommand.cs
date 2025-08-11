using MediatR;

namespace TaskMate.Application.Projects.CreateProject
{
    public class CreateProjectCommand : IRequest<int>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
