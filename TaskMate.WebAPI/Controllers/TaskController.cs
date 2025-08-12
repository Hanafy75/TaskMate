using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMate.Application.Dtos;
using TaskMate.Application.TaskItems.CreateTaskItem;
using TaskMate.Application.TaskItems.DeleteTaskItem;
using TaskMate.Application.TaskItems.GetTaskItem;
using TaskMate.Application.TaskItems.UpdateTaskITem;
using TaskMate.WebAPI.Responses;

namespace TaskMate.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create(CreateTaskItemCommand command)
        {
            var result = await _mediator.Send(command);

            return Ok(ApiResponse<int>.Success(result));
        }

        [HttpGet("{Id}")]
        public async Task<ActionResult<ApiResponse<TaskItemDto>>> Get(int Id)
        {
            var result = await _mediator.Send(new GetTaskItemQuery { Id = Id});

            return Ok(ApiResponse<TaskItemDto>.Success(result));
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<object>>> Update(UpdateTaskITemCommand command)
        {
             await _mediator.Send(command);

            return Ok(ApiResponse<object>.Success(null,"Task Updated Successfully"));
        }
        [HttpDelete]
        public async Task<ActionResult<ApiResponse<object>>> Delete(DeleteTaskItemCommand command)
        {
             await _mediator.Send(command);

            return Ok(ApiResponse<object>.Success(null,"Task Updated Successfully"));
        }
    }
}
