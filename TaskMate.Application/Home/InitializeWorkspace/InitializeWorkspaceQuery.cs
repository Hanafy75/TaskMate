using MediatR;
using TaskMate.Application.Dtos;

namespace TaskMate.Application.Home.InitializeWorkspace
{
    public class InitializeWorkspaceQuery : IRequest<HomeDto>
    {
    }
}
