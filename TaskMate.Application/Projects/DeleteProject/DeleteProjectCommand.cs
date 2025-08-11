using MediatR;

namespace TaskMate.Application.Projects.DeleteProject
{
    public class DeleteProjectCommand :IRequest
    {
        public int Id { get; set; }
    }
}
