using MediatR;
using TaskMate.Application.Dtos;

namespace TaskMate.Application.TaskItems.GetTaskItems
{
    public class GetBoardTaskItemsQuery : IRequest<IEnumerable<TaskItemDto>>
    {
        public int BoardId { get; set; }
    }
}
