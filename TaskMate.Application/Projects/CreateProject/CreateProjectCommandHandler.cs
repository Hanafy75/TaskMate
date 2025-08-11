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

            var project = new Project 
            {
                Name = request.Name,
                Description = request.Description,
                UserId = userId!
            };
            await _projectRepo.AddAsync(project);
            await _unitOfWork.SaveChangesAsync();
            return project.Id;
        }
    }
}
