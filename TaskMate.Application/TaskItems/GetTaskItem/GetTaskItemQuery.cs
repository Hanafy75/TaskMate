using MediatR;
using TaskMate.Application.Dtos;

namespace TaskMate.Application.TaskItems.GetTaskItem
{
    public class GetTaskItemQuery : IRequest<TaskItemDto>
    {
        public int Id { get; set; }
    }
}
