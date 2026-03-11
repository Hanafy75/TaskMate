using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMate.Application.Dtos;
using TaskMate.Application.Home.GetAllBoards;
using TaskMate.Application.Home.InitializeWorkspace;
using TaskMate.WebAPI.Common;

namespace TaskMate.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class HomeController(IMediator mediator) : BaseApiController
    {
        [HttpGet("Projects")]
        public async Task<IActionResult> GetAllProjects(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetAllProjectsQuery(), cancellationToken);
            return result.IsSucceeded ? Ok(result.Value) : MapErrors(result.Errors);
        }

        [HttpGet("Boards")]
        public async Task<IActionResult> InitializeWorkspace(CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetAllBoardsQuery(), cancellationToken);
            return result.IsSucceeded ? Ok(result.Value) : MapErrors(result.Errors);
        }
    }
}
