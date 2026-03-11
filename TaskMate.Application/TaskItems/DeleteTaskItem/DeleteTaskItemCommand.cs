using MediatR;

namespace TaskMate.Application.TaskItems.DeleteTaskItem
{

    public class DeleteTaskItemCommand : IRequest<Result>
    {
        public int Id { get; set; }
    }
}
