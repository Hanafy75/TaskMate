using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskMate.Application.Dtos;
using TaskMate.Application.User.CreateUser;
using TaskMate.Application.User.LoginUser;
using TaskMate.Application.User.RefreshToken;
using TaskMate.Application.User.RevokeToken;
using TaskMate.Domain.ValueObject;
using TaskMate.WebAPI.Responses;

namespace TaskMate.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator _mediator) : ControllerBase
    {

        [HttpPost("Register")]
        public async Task<ActionResult<ApiResponse<AuthResult>>> Register([FromBody] CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            //set the refresh token in the cookie
            SetRefreshTokenInCookies(result.RefreshToken, result.RefreshTokenExpiresOn);
            return Ok(ApiResponse<AuthResult>.Success(result));
        }

        [HttpPost("Login")]
        public async Task<ActionResult<ApiResponse<AuthResult>>> Login([FromBody] LoginUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookies(result.RefreshToken, result.RefreshTokenExpiresOn);

            return Ok(ApiResponse<AuthResult>.Success(result));
        }

        [HttpPost("RevokeRefreshToken")]
        public async Task<ActionResult<ApiResponse<object>>> Revoke([FromBody] RevokeTokenCommand? command)
        {

            if (command is null)
            {
                command = new RevokeTokenCommand();
                command.RefreshToken = Request.Cookies[CookieKeys.RefreshToken];
            }

            var result = await _mediator.Send(command);


            return Ok(ApiResponse<object>.Success(null, "Token Revoked Successfully"));
        }


        [HttpGet("Refresh")]
        public async Task<ActionResult<ApiResponse<AuthResult>>> Refresh()
        {
            var refreshToken = Request.Cookies[CookieKeys.RefreshToken] ?? string.Empty;

            var command = new RefreshTokenCommand() { RefreshToken = refreshToken };

            var result = await _mediator.Send(command);

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookies(result.RefreshToken, result.RefreshTokenExpiresOn);

            return Ok(ApiResponse<AuthResult>.Success(result));
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
