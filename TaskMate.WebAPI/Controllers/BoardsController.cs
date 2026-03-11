using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMate.Application.Boards.CreateBoard;
using TaskMate.Application.Boards.DeleteBoard;
using TaskMate.Application.Boards.GetBoard;
using TaskMate.Application.Boards.GetProjectBoards.GetAllBoards;
using TaskMate.Application.Boards.UpdateBoard;
using TaskMate.Application.Dtos;
using TaskMate.WebAPI.Common;

namespace TaskMate.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class BoardsController(IMediator mediator) : BaseApiController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateBoardCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.IsSucceeded
                ? CreatedAtAction(nameof(Get), new { Id = result.Value }, result.Value)
                : MapErrors(result.Errors);
        }

        [HttpGet("/api/Projects/{Id}/Boards")]
        public async Task<IActionResult> GetAll(int Id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetProjectBoardsQuery { ProjectId= Id }, cancellationToken);
            return result.IsSucceeded ? Ok(result.Value) : MapErrors(result.Errors);
        }


        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetBoardQuery { Id = Id }, cancellationToken);
            return result.IsSucceeded ? Ok(result.Value) : MapErrors(result.Errors);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateBoardCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            return result.IsSucceeded ? NoContent() : MapErrors(result.Errors);
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(int Id, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new DeleteBoardCommand { Id = Id }, cancellationToken);
            return result.IsSucceeded ? NoContent() : MapErrors(result.Errors);
        }
    }
}
