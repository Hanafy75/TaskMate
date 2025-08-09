using Microsoft.EntityFrameworkCore;
using TaskMate.Application.Dtos;
using TaskMate.Application.IRepositories;
using TaskMate.Domain.Entities;
using TaskMate.Infrastructure.Persistence;

namespace TaskMate.Infrastructure.Repositories
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<ProjectDto>> GetProjectDtosAsync(string userId)
        {
            var projectsDto = await _context.Projects.Where(p => p.UserId == userId)
                .Select(p=> new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    CreatedAt = p.CreatedAt,
                }).ToListAsync();
            return projectsDto;
        }
    }
}
