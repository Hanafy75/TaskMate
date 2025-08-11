using MediatR;
using TaskMate.Application.Exceptions;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.Projects.DeleteProject
{
    public class DeleteProjectCommandHandler(IProjectRepository _projectRepo, IUserService _userService, IUnitOfWork _unitOfWork) : IRequestHandler<DeleteProjectCommand>
    {
        public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            var projectFromDb = await _projectRepo.GetByIdAsync(request.Id);

            if (projectFromDb is null) throw new NotFoundException($"Project with id {request.Id} that you are trying to delete does not exist");

            //check ownership for the requested user
            if (projectFromDb.UserId != userId) throw new ForbiddenException("you don't have permission to access this resource");

             _projectRepo.Delete(projectFromDb);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
