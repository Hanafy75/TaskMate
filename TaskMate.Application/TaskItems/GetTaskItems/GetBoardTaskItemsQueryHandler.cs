using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Exceptions;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Entities;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.TaskItems.GetTaskItems
{
    public class GetBoardTaskItemsQueryHandler(
        IBoardRepository _boardRepo,
        IProjectRepository _projectRepo,
        IGenericRepository<TaskItem> _taskRepo,
        IUserService _userService) : IRequestHandler<GetBoardTaskItemsQuery, IEnumerable<TaskItemDto>>
    {
        public async Task<IEnumerable<TaskItemDto>> Handle(GetBoardTaskItemsQuery request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            var board = await _boardRepo.GetByIdAsync(request.BoardId);

            if (board is null)
            {
                throw new NotFoundException("Task not found.");
            }


            bool hasAccess = await CheckUserAccess(board, userId);

            if (!hasAccess)
            {
                throw new ForbiddenException("You do not have permission to access this resource.");
            }

            var tasks = (await _taskRepo.GetAllAsync(t => t.BoardId == board.Id))
                .Select(task => new TaskItemDto
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    CreatedAt = task.CreatedAt,
                    DueDate = task.DueDate,
                    Status = task.Status,
                });

            return tasks;
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
