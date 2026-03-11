using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMate.Application.Dtos;
using TaskMate.Application.TaskItems.CreateTaskItem;
using TaskMate.Application.TaskItems.DeleteTaskItem;
using TaskMate.Application.TaskItems.GetTaskItem;
using TaskMate.Application.TaskItems.GetTaskItems;
using TaskMate.Application.TaskItems.UpdateTaskITem;
using TaskMate.WebAPI.Common;

namespace TaskMate.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class TaskController(IMediator mediator) : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskItemCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.IsSucceeded ? Ok(result.Value) : MapErrors(result.Errors);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetTaskItemQuery { Id = Id}, cancellationToken);
            return result.IsSucceeded ? Ok(result.Value) : MapErrors(result.Errors);
        }

        [HttpGet("/api/Boards/{Id}/Tasks")]
        public async Task<IActionResult> GetAll(int Id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetBoardTaskItemsQuery { BoardId = Id}, cancellationToken);
            return result.IsSucceeded ? Ok(result.Value) : MapErrors(result.Errors);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateTaskITemCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.IsSucceeded ? NoContent() : MapErrors(result.Errors);
        }
        [HttpDelete("{Id:int}")]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteTaskItemCommand { Id = Id }, cancellationToken);
            return result.IsSucceeded ? NoContent() : MapErrors(result.Errors);
        }
    }
}
