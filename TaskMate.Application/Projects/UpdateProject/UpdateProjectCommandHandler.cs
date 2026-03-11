using MediatR;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.Projects.UpdateProject
{
    internal sealed class UpdateProjectCommandHandler(
        IProjectRepository projectRepo,
        IUserService userService,
        IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateProjectCommand, Result>
    {
        public async Task<Result> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = userService.GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Error.Unauthorized("Auth.Unauthorized", "Authentication is required.");

            var projectFromDb = await projectRepo.GetByIdAsync(request.Id, cancellationToken);
            if (projectFromDb is null)
                return Error.NotFound("Project.NotFound", $"Project with id {request.Id} was not found.");

            //check ownership for the requested user
            if (projectFromDb.UserId != userId)
                return Error.Forbidden("Project.Forbidden", "You don't have permission to access this resource");

            //update the date
            projectFromDb.Name = request.Name;
            projectFromDb.Description = request.Description;

            projectRepo.Update(projectFromDb);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Ok();
        }
    }
}
