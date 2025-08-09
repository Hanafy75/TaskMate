using Microsoft.EntityFrameworkCore;
using TaskMate.Application.Dtos;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Entities;
using TaskMate.Infrastructure.Persistence;

namespace TaskMate.Infrastructure.Repositories
{
    public class BoardRepository : GenericRepository<Board>, IBoardRepository
    {
        public BoardRepository(AppDbContext context):base(context) { }

        public async Task<IEnumerable<BoardDto>> GetIndependentBoardDtoAsync(string userId)
        {
            var boardsDto = await _context.Boards.Where(b=>b.UserId==userId).
                Select(b=> new BoardDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    CreatedAt = b.CreatedAt,
                }).ToListAsync();

            return boardsDto;
        }
    }
}
