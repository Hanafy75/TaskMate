using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;

namespace TaskMate.Application.Home.InitializeWorkspace
{
    internal sealed class GetAllProjectsQueryHandler(
        IProjectRepository projectRepo,
        IUserService userService)
        : IRequestHandler<GetAllProjectsQuery, Result<IEnumerable<ProjectDto>>>
    {
        public async Task<Result<IEnumerable<ProjectDto>>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = userService.GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(currentUserId))
                return Error.Unauthorized("Auth.Unauthorized", "User must be logged in.");

            var projects = await projectRepo.GetProjectDtosAsync(currentUserId, cancellationToken);
            return Result<IEnumerable<ProjectDto>>.Ok(projects);
        }
    }
}
