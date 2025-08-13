using MediatR;
using TaskMate.Application.Dtos;

namespace TaskMate.Application.Home.InitializeWorkspace
{
    public class GetAllProjectsQuery : IRequest<IEnumerable<ProjectDto>>
    {
    }
}
