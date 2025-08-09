using TaskMate.Application.Dtos;
using TaskMate.Domain.Entities;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.IRepositories
{
    public interface IBoardRepository : IGenericRepository<Board>
    {
        Task<IEnumerable<BoardDto>> GetIndependentBoardDtoAsync(string userId);
    }
}
