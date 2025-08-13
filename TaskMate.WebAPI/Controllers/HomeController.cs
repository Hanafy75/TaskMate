using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMate.Application.Dtos;
using TaskMate.Application.Home.GetAllBoards;
using TaskMate.Application.Home.InitializeWorkspace;
using TaskMate.WebAPI.Responses;

namespace TaskMate.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HomeController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("Projects")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ProjectDto>>>> GetAllProjects()
        {
            var result = await _mediator.Send(new GetAllProjectsQuery());
            return Ok(ApiResponse<IEnumerable<ProjectDto>>.Success(result));
        }

        [HttpGet("Boards")]
        public async Task<ActionResult<ApiResponse<IEnumerable<BoardDto>>>> InitializeWorkspace()
        {
            var result = await _mediator.Send(new GetAllBoardsQuery());
            return Ok(ApiResponse<IEnumerable<BoardDto>>.Success(result));
        }
    }
}
