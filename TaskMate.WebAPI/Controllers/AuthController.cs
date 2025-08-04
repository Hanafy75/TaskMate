using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskMate.Application.Dtos;
using TaskMate.Application.User.CreateUser;
using TaskMate.WebAPI.Responses;

namespace TaskMate.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("Register")]
        public async Task<ActionResult<ApiResponse<AuthResult>>> Register(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(ApiResponse<AuthResult>.Success(result));
        }
    }
}
