using MediatR;

namespace TaskMate.Application.Projects.DeleteProject
{
    public class DeleteProjectCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}
