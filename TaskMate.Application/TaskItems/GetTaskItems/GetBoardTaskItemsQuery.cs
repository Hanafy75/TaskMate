using MediatR;
using TaskMate.Application.Dtos;

namespace TaskMate.Application.TaskItems.GetTaskItems
{
    public class GetBoardTaskItemsQuery : IRequest<Result<IEnumerable<TaskItemDto>>>
    {
        public int BoardId { get; set; }
    }
}
