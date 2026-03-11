using MediatR;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Entities;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.Boards.CreateBoard
{
    internal sealed class CreateBoardCommandHandler(
        IBoardRepository boardRepo,
        IProjectRepository projectRepo,
        IUserService userService,
        IUnitOfWork unitOfWork)
        : IRequestHandler<CreateBoardCommand, Result<int>>
    {
        public async Task<Result<int>> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
        {
            //get current user id
            var userId = userService.GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(userId))
                return Error.Unauthorized("Auth.Unauthorized", "Authentication is required.");

            int boardId;
            //check if it's independent Board or belongs to a project
            if (!request.ProjectId.HasValue)
            {
                // this means it's independent so we add it to user 
                var board = new Board
                {
                    Name = request.Name,
                    Description = request.Description,
                    UserId = userId,
                };
                await boardRepo.AddAsync(board, cancellationToken);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                boardId = board.Id;
            }
            else
            {
                // so it is belong to a project so we need to get the project to check the ownership fo the current logged in user
                var project = await projectRepo.GetByIdAsync(request.ProjectId.Value, cancellationToken);

                if (project is null)
                    return Error.NotFound("Project.NotFound", "The project that this board belongs to does not exist.");

                //check the owenership
                if (project.UserId != userId)
                    return Error.Forbidden("Board.Forbidden", "You can't add a board to a project you don't have access to.");

                // if we get here so the project exist and user has access to it  so we add the board to this project
                var board = new Board
                {
                    Name = request.Name,
                    Description = request.Description,
                };
                project.Boards.Add(board);
                await unitOfWork.SaveChangesAsync(cancellationToken);
                boardId = board.Id;
            }

            return boardId;
        }
    }
}
