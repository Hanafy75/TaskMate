using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Entities;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.TaskItems.UpdateTaskITem
{
    internal sealed class UpdateTaskITemCommandHandler(
        IBoardRepository boardRepo,
        IProjectRepository projectRepo,
        IGenericRepository<TaskItem> taskRepo,
        IUserService userService,
        IUnitOfWork unitOfWork)
        : IRequestHandler<UpdateTaskITemCommand, Result>
    {
        public async Task<Result> Handle(UpdateTaskITemCommand request, CancellationToken cancellationToken)
        {
            var userId = userService.GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Error.Unauthorized("Auth.Unauthorized", "Authentication is required.");

            var task = await taskRepo.GetByIdAsync(request.Id, cancellationToken);
            if (task is null)
            {
                return Error.NotFound("TaskItem.NotFound", "Task not found.");
            }

            var board = await boardRepo.GetByIdAsync(task.BoardId, cancellationToken);
            if (board is null)
            {
                // This scenario implies a data integrity issue, as a task should not exist without its board.
                return Error.NotFound("Board.NotFound", "The board associated with this task could not be found.");
            }

            var accessResult = await CheckUserAccess(board, userId, cancellationToken);
            if (accessResult.IsFailed)
                return accessResult.Errors.ToList();

            if (!accessResult.Value)
                return Error.Forbidden("TaskItem.Forbidden", "You do not have permission to access this resource.");

            task.Name = request.Name;
            task.Description = request.Description;
            task.DueDate = request.DueDate;
            task.Status = request.Status;

            taskRepo.Update(task);
            await unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }
        private async Task<Result<bool>> CheckUserAccess(
            Board board,
            string userId,
            CancellationToken cancellationToken)
        {
            // Case 1: The board is an independent board belonging to the user.
            if (board.UserId is not null)
            {
                return board.UserId == userId;
            }

            // Case 2: The board belongs to a project.
            if (board.ProjectId is not null)
            {
                var project = await projectRepo.GetByIdAsync(board.ProjectId.Value, cancellationToken);
                // Check if the project exists and if the user owns it.
                return project?.UserId == userId;
            }

            // If the board has neither a UserId nor a ProjectId, it's an invalid state.
            return Error.Failure(
                "Board.InvalidState",
                "The board is not correctly configured as it does not belong to a user or a project.");
        }
    }
}
