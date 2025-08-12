using MediatR;

namespace TaskMate.Application.TaskItems.DeleteTaskItem
{

    public class DeleteTaskItemCommand : IRequest
    {
        public int Id { get; set; }
    }
}
