using MediatR;
using TaskMate.Application.Exceptions;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Entities;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.Boards.CreateBoard
{
    public class CreateBoardCommandHandler(IBoardRepository _boardRepo, IProjectRepository _projectRepo, IUserService _userService, IUnitOfWork _unitOfWork) : IRequestHandler<CreateBoardCommand, int>
    {
        public async Task<int> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
        {
            //get current user id
            var userId = _userService.GetCurrentUserId();

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
                await _boardRepo.AddAsync(board);
                await _unitOfWork.SaveChangesAsync();
                boardId = board.Id;
            }
            else
            {
                // so it is belong to a project so we need to get the project to check the ownership fo the current logged in user
                var project = await _projectRepo.GetByIdAsync(request.ProjectId.Value);

                if (project is null) throw new NotFoundException("The project that this board belongs to does not exist");

                //check the owenership
                if (project.UserId != userId) throw new ForbiddenException("you can't add board to a project you don't have access to it");

                // if we get here so the project exist and user has access to it  so we add the board to this project
                var board = new Board
                {
                    Name = request.Name,
                    Description = request.Description,
                };
                project.Boards.Add(board);
                await _unitOfWork.SaveChangesAsync();
                boardId = board.Id;
            }

            return boardId;
        }
    }
}
