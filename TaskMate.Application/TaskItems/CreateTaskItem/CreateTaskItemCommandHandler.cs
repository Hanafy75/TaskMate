using MediatR;
using TaskMate.Application.Exceptions;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Entities;
using TaskMate.Domain.Enums;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.TaskItems.CreateTaskItem
{
    public class CreateTaskItemCommandHandler(IBoardRepository _boardRepo, IProjectRepository _ProjectRepo, IUserService _userService, IUnitOfWork _unitOfWork) : IRequestHandler<CreateTaskItemCommand, int>
    {
        public async Task<int> Handle(CreateTaskItemCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();

            var board = await _boardRepo.GetByIdAsync(request.BoardId);

            if (board is null) throw new NotFoundException("The board which is contains this Task does not exist");

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
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    throw new ForbiddenException("you have no access to do this operation on this resource");
                }
                
            }
            else if(board.ProjectId is not null )
            {
                // 2nd case belongs to project
                var project = await _ProjectRepo.GetByIdAsync(board.ProjectId.Value);

                //check if the user has this project
                if(project.UserId == userId)
                {
                    board.Tasks.Add(task);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
            else
            {
                throw new BadRequestException("The board that this task belongs to does not belong to any user or any project ");
            }

            return task.Id;
        }
    }
}
