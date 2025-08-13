using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMate.Application.Boards.CreateBoard;
using TaskMate.Application.Boards.DeleteBoard;
using TaskMate.Application.Boards.GetBoard;
using TaskMate.Application.Boards.GetProjectBoards.GetAllBoards;
using TaskMate.Application.Boards.UpdateBoard;
using TaskMate.Application.Dtos;
using TaskMate.Application.Projects.DeleteProject;
using TaskMate.WebAPI.Responses;

namespace TaskMate.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BoardsController(IMediator _mediator) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<ApiResponse<int>>> Create(CreateBoardCommand command)
        {
            var result = await _mediator.Send(command);

            var response = ApiResponse<int>.Success(result, "Board Created Successfully");

            return CreatedAtAction(nameof(Get), new { Id = result }, response);
        }

        [HttpGet("/api/Projects/{Id}/Boards")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BoardDto>>>> GetAll(int Id)
        {
            var result = await _mediator.Send(new GetProjectBoardsQuery { ProjectId= Id });

            return Ok(ApiResponse<IEnumerable<BoardDto>>.Success(result));
        }


        [HttpGet("{Id}")]
        public async Task<ActionResult<ApiResponse<BoardDto>>> Get(int Id)
        {
            var result = await _mediator.Send(new GetBoardQuery { Id = Id });

            return Ok(ApiResponse<BoardDto>.Success(result));
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<object>>> Update(UpdateBoardCommand command)
        {
            await _mediator.Send(command);
            return Ok(ApiResponse<object>.Success(null, "Board Updated Successfully"));
        }

        [HttpDelete("{Id}")]
        public async Task<ActionResult<ApiResponse<object>>> Delete(int Id)
        {
            await _mediator.Send(new DeleteBoardCommand { Id = Id });

            return Ok(ApiResponse<object>.Success(null, "Board Deleted Successfully"));
        }
    }
}
