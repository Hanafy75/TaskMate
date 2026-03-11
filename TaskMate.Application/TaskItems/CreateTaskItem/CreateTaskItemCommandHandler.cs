using MediatR;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Entities;
using TaskMate.Domain.Enums;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.TaskItems.CreateTaskItem
{
    internal sealed class CreateTaskItemCommandHandler(
        IBoardRepository boardRepo,
        IProjectRepository projectRepo,
        IUserService userService,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CreateTaskItemCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
        {
            var userId = userService.GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Error.Unauthorized("Auth.Unauthorized", "Authentication is required.");

            var board = await boardRepo.GetByIdAsync(request.BoardId, cancellationToken);

            if (board is null)
                return Error.NotFound("Board.NotFound", "The board that contains this task does not exist.");

            var task = new TaskItem
            {
                Name = request.Name,
                Description = request.Description,
                DueDate = request.DueDate,
                Status = TaskState.Todo,
            };

            // 2 cases , 
            if(board.UserId is not null ) // this means the board is independent and belongs to the current user
            {
                // 1st case is independent
                if(board.UserId == userId)
                {
                    board.Tasks.Add(task);
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    return Error.Forbidden("TaskItem.Forbidden", "You have no access to do this operation on this resource.");
                }
                
            }
            else if(board.ProjectId is not null )
            {
                // 2nd case belongs to project
                var project = await projectRepo.GetByIdAsync(board.ProjectId.Value, cancellationToken);

                //check if the user has this project
                if(project?.UserId == userId)
                {
                    board.Tasks.Add(task);
                    await unitOfWork.SaveChangesAsync(cancellationToken);
                }
                else
                {
                    return Error.Forbidden("TaskItem.Forbidden", "You have no access to do this operation on this resource.");
                }
            }
            else
            {
                return Error.Failure(
                    "Board.InvalidState",
                    "The board that this task belongs to does not belong to any user or any project.");
            }

            return task.Id;
        }
    }
}
