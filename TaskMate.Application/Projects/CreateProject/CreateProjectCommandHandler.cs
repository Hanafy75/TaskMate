using MediatR;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Entities;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.Projects.CreateProject
{
    public class CreateProjectCommandHandler(IProjectRepository _projectRepo, IUserService _userService, IUnitOfWork _unitOfWork) : IRequestHandler<CreateProjectCommand, int>
    {
        public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();// the auth filter will catch if this is null so no need to check

            var project = new Project //this conflict with the name space (this is soppose to be an entity)
            {
                Name = request.Name,
                Description = request.Description,
                CreatedAt = DateTime.UtcNow,
                UserId = userId!
            };
            await _projectRepo.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();
            return project.Id;
        }
    }
}
