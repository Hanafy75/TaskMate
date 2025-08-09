using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskMate.Application.Dtos;
using TaskMate.Application.Home.InitializeWorkspace;
using TaskMate.WebAPI.Responses;

namespace TaskMate.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController(IMediator _mediator) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<ApiResponse<HomeDto>>> InitializeWorkspace()
        {
            var result = await _mediator.Send(new InitializeWorkspaceQuery());
            return Ok(ApiResponse<HomeDto>.Success(result));
        }
    }
}
