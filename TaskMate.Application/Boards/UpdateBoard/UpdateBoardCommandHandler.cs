using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Exceptions;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.Boards.UpdateBoard
{
    public class UpdateBoardCommandHandler(IBoardRepository _boardRepo, IProjectRepository _ProjectRepo, IUserService _userService, IUnitOfWork _unitOfWork) : IRequestHandler<UpdateBoardCommand>
    {
        public async Task Handle(UpdateBoardCommand request, CancellationToken cancellationToken)
        {
            var userId = _userService.GetCurrentUserId();
            // we retreive the board
            var board = await _boardRepo.GetByIdAsync(request.Id);

            if (board is null) throw new NotFoundException($"Board with id {request.Id} does not exist");

            // we have 2 scenarios => 1. board belongs to project / 2. board belongs to user (independent)
            if (board.ProjectId.HasValue)
            {
                // this means we are in case 1 / we need to get the project to check if the current user has access to it or not
                var project = await _ProjectRepo.GetByIdAsync(board.ProjectId.Value);

                if (project is null) throw new NotFoundException($"Project with id {board.ProjectId.Value} does not exist");

                if (project.UserId != userId) throw new ForbiddenException("you don't have access to this board");

                board.Name = request.Name;
                board.Description = request.Description;
                _boardRepo.Update(board);
                await _unitOfWork.SaveChangesAsync();

            }
            else
            {
                // this means we are in case 2
                if (board.UserId != userId) throw new ForbiddenException("you don't have access to this board");

                board.Name = request.Name;
                board.Description = request.Description;
                _boardRepo.Update(board);
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
