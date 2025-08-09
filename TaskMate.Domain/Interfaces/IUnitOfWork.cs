namespace TaskMate.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();
    }
}
