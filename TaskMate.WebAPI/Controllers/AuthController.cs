using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskMate.Application.Constants;
using TaskMate.Application.Dtos;
using TaskMate.Application.User.CreateUser;
using TaskMate.Application.User.LoginUser;
using TaskMate.Application.User.RefreshToken;
using TaskMate.Application.User.RevokeToken;
using TaskMate.WebAPI.Common;

namespace TaskMate.WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class AuthController(IMediator mediator) : BaseApiController
    {
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);
            if (result.IsFailed)
                return MapErrors(result.Errors);

            SetRefreshTokenInCookies(result.Value.RefreshToken, result.Value.RefreshTokenExpiresOn);
            return Ok(result.Value);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailed)
                return MapErrors(result.Errors);

            if (!string.IsNullOrEmpty(result.Value.RefreshToken))
                SetRefreshTokenInCookies(result.Value.RefreshToken, result.Value.RefreshTokenExpiresOn);

            return Ok(result.Value);
        }

        [HttpPost("RevokeRefreshToken")]
        public async Task<IActionResult> Revoke([FromBody] RevokeTokenCommand? command, CancellationToken cancellationToken)
        {

            if (command is null)
            {
                command = new RevokeTokenCommand();
                command.RefreshToken = Request.Cookies[CookieKeys.RefreshToken];
            }

            var result = await mediator.Send(command, cancellationToken);
            return result.IsSucceeded ? NoContent() : MapErrors(result.Errors);
        }


        [HttpGet("Refresh")]
        public async Task<IActionResult> Refresh(CancellationToken cancellationToken)
        {
            var refreshToken = Request.Cookies[CookieKeys.RefreshToken] ?? string.Empty;

            var command = new RefreshTokenCommand() { RefreshToken = refreshToken };

            var result = await mediator.Send(command, cancellationToken);

            if (result.IsFailed)
                return MapErrors(result.Errors);

            if (!string.IsNullOrEmpty(result.Value.RefreshToken))
                SetRefreshTokenInCookies(result.Value.RefreshToken, result.Value.RefreshTokenExpiresOn);

            return Ok(result.Value);
        }

        #region Helpers
        private void SetRefreshTokenInCookies(string refreshToken, DateTime expires)
        {
            var cookieoptions = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                Expires = expires
            };

            Response.Cookies.Append(CookieKeys.RefreshToken, refreshToken);
        }
        #endregion
    }
}
