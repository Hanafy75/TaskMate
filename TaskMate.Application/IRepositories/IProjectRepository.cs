using TaskMate.Application.Dtos;
using TaskMate.Domain.Entities;
using TaskMate.Domain.Interfaces;

namespace TaskMate.Application.IRepositories
{
    public interface IProjectRepository : IGenericRepository<Project>
    {
        Task<IEnumerable<ProjectDto>> GetProjectDtosAsync(string userId);
    }
}
