using Mapster;
using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Exceptions;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;

namespace TaskMate.Application.Projects.GetProject
{
    public class GetProjectQueryHandler(IProjectRepository _projectRepo, IUserService _userService) : IRequestHandler<GetProjectQuery, ProjectDto>
    {
        public async Task<ProjectDto> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            var userID = _userService.GetCurrentUserId();

            var project = await _projectRepo.GetByIdAsync(request.Id);

            // check if it's exist
            if (project is null) throw new NotFoundException($"Project with id {request.Id} not found");

            //check ownership for the requested user
            if(project.UserId != userID) throw new ForbiddenException("you don't have permission to access this resource");

            return project.Adapt<ProjectDto>();
        }
    }
}
