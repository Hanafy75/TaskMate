using MediatR;
using TaskMate.Application.Dtos;
using TaskMate.Application.Interfaces;
using TaskMate.Application.IRepositories;

namespace TaskMate.Application.Home.InitializeWorkspace
{
    internal class InitializeWorkspaceQueryHandler(IBoardRepository _boardRepo, IProjectRepository _projectRepo, IUserService _userService)
        : IRequestHandler<InitializeWorkspaceQuery, HomeDto>
    {
        public async Task<HomeDto> Handle(InitializeWorkspaceQuery request, CancellationToken cancellationToken)
        {
            var currentUserId = _userService.GetCurrentUserIdAsync();

            if (currentUserId is null) throw new UnauthorizedAccessException("user must be logged in.");

            var homeDto = new HomeDto
            {
                Projects = await _projectRepo.GetProjectDtosAsync(currentUserId),
                boards = await _boardRepo.GetIndependentBoardDtoAsync(currentUserId)
            };
            return homeDto;
        }
    }
}
