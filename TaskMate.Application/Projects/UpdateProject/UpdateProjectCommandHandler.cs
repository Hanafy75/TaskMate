using MediatR;
using TaskMate.Application.Exceptions;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.Projects.UpdateProject
{
    public class UpdateProjectCommandHandler(IProjectRepository _projectRepo, IUserService _userService, IUnitOfWork _unitOfWork) : IRequestHandler<UpdateProjectCommand>
    {
        public async Task Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            var projectFromDb = await _projectRepo.GetByIdAsync(request.Id);
            if (projectFromDb is null) throw new NotFoundException($"Project with id {request.Id} that you are trying to update does not exist");

            //check ownership for the requested user
            if (projectFromDb.UserId != userId) throw new ForbiddenException("you don't have permission to access this resource");

            //update the date
            projectFromDb.Name = request.Name;
            projectFromDb.Description = request.Description;

            _projectRepo.Update(projectFromDb);
            await _unitOfWork.SaveChangesAsync();

        }
    }
}
