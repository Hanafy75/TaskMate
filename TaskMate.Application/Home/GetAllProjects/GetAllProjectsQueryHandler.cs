using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;

namespace TaskMate.Application.Home.InitializeWorkspace
{
    internal class GetAllProjectsQueryHandler(IBoardRepository _boardRepo, IProjectRepository _projectRepo, IUserService _userService)
        : IRequestHandler<GetAllProjectsQuery, IEnumerable<ProjectDto>>
    {
        public async Task<IEnumerable<ProjectDto>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _userService.GetCurrentUserId();

            if (currentUserId is null) throw new UnauthorizedAccessException("user must be logged in.");

            var Projects = await _projectRepo.GetProjectDtosAsync(currentUserId);
            return Projects;
        }
    }
}
