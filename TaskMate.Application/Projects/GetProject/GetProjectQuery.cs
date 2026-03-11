using MediatR;
using TaskMate.Application.Dtos;

namespace TaskMate.Application.Projects.GetProject
{
    public class GetProjectQuery : IRequest<Result<ProjectDto>>
    {
        public int Id { get; set; }
    }
}
