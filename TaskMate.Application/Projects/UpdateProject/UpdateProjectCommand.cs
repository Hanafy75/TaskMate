using MediatR;
using TaskMate.Application.Dtos;

namespace TaskMate.Application.Projects.UpdateProject
{
    public class UpdateProjectCommand : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
    }
}
