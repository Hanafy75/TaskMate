using MediatR;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Entities;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.Projects.CreateProject
{
    internal sealed class CreateProjectCommandHandler(
        IProjectRepository projectRepo,
        IUserService userService,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CreateProjectCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = userService.GetCurrentUserId();// the auth filter will catch if this is null so no need to check
            if (string.IsNullOrWhiteSpace(userId))
                return Error.Unauthorized("Auth.Unauthorized", "Authentication is required.");

            var project = new Project 
            {
                Name = request.Name,
                Description = request.Description,
                UserId = userId!
            };
            await projectRepo.AddAsync(project, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return project.Id;
        }
    }
}
