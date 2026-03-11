using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Entities;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.TaskItems.GetTaskItems
{
    internal sealed class GetBoardTaskItemsQueryHandler(
        IBoardRepository boardRepo,
        IProjectRepository projectRepo,
        IGenericRepository<TaskItem> taskRepo,
        IUserService userService)
        : IRequestHandler<GetBoardTaskItemsQuery, Result<IEnumerable<TaskItemDto>>>
    {
        public async Task<Result<IEnumerable<TaskItemDto>>> Handle(GetBoardTaskItemsQuery request, CancellationToken cancellationToken)
        {
            var userId = userService.GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Error.Unauthorized("Auth.Unauthorized", "Authentication is required.");

            var board = await boardRepo.GetByIdAsync(request.BoardId);

            if (board is null)
            {
                return Error.NotFound("Board.NotFound", "Board not found.");
            }


            var accessResult = await CheckUserAccess(board, userId, cancellationToken);
            if (accessResult.IsFailed)
                return accessResult.Errors.ToList();

            if (!accessResult.Value)
                return Error.Forbidden("Board.Forbidden", "You do not have permission to access this resource.");

            var tasks = (await taskRepo.GetAllAsync(t => t.BoardId == board.Id))
                .Select(task => new TaskItemDto
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    CreatedAt = task.CreatedAt,
                    DueDate = task.DueDate,
                    Status = task.Status,
                });

            return Result<IEnumerable<TaskItemDto>>.Ok(tasks);
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
