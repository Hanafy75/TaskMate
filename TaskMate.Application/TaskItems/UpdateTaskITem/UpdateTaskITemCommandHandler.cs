using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Exceptions;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Entities;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.TaskItems.UpdateTaskITem
{
    public class UpdateTaskITemCommandHandler(
        IBoardRepository _boardRepo,
        IProjectRepository _projectRepo,
        IGenericRepository<TaskItem> _taskRepo,
        IUserService _userService,
        IUnitOfWork _unitOfWork) : IRequestHandler<UpdateTaskITemCommand>
    {
        public async Task Handle(UpdateTaskITemCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            var task = await _taskRepo.GetByIdAsync(request.Id);
            if (task is null)
            {
                throw new NotFoundException("Task not found.");
            }

            var board = await _boardRepo.GetByIdAsync(task.BoardId);
            if (board is null)
            {
                // This scenario implies a data integrity issue, as a task should not exist without its board.
                throw new NotFoundException("The board associated with this task could not be found.");
            }

            bool hasAccess = await CheckUserAccess(board, userId);

            if (!hasAccess)
            {
                throw new ForbiddenException("You do not have permission to access this resource.");
            }

            task.Name = request.Name;
            task.Description = request.Description;
            task.DueDate = request.DueDate;
            task.Status = request.Status;

            _taskRepo.Update(task);
            await _unitOfWork.SaveChangesAsync();
        }
        private async Task<bool> CheckUserAccess(Board board, string userId)
        {
            // Case 1: The board is an independent board belonging to the user.
            if (board.UserId is not null)
            {
                return board.UserId == userId;
            }

            // Case 2: The board belongs to a project.
            if (board.ProjectId is not null)
            {
                var project = await _projectRepo.GetByIdAsync(board.ProjectId.Value);
                // Check if the project exists and if the user owns it.
                return project?.UserId == userId;
            }

            // If the board has neither a UserId nor a ProjectId, it's an invalid state.
            throw new BadRequestException("The board is not correctly configured as it does not belong to a user or a project.");
        }
    }
}
