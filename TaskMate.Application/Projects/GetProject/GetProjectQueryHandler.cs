using Mapster;
using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;

namespace TaskMate.Application.Projects.GetProject
{
    internal sealed class GetProjectQueryHandler(
        IProjectRepository projectRepo,
        IUserService userService)
        : IRequestHandler<GetProjectQuery, Result<ProjectDto>>
    {
        public async Task<Result<ProjectDto>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            var userID = userService.GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(userID))
                return Error.Unauthorized("Auth.Unauthorized", "Authentication is required.");

            var project = await projectRepo.GetByIdAsync(request.Id, cancellationToken);

            // check if it's exist
            if (project is null)
                return Error.NotFound("Project.NotFound", $"Project with id {request.Id} was not found.");

            //check ownership for the requested user
            if(project.UserId != userID)
                return Error.Forbidden("Project.Forbidden", "You don't have permission to access this resource.");

            return project.Adapt<ProjectDto>();
        }
    }
}
