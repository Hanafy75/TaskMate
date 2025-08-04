using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Security.Claims;
using TaskMate.Application.Dtos;
using TaskMate.Application.Exceptions;
using TaskMate.Application.Interfaces;
using TaskMate.Application.Options;
using TaskMate.Application.User.CreateUser;
using TaskMate.Domain.Entities;

namespace TaskMate.Application.Services
{
    internal class AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ITokenService tokenService,IOptions<JWTOptions> options) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IOptions<JWTOptions> _options = options;

        public Task<AuthResult> GetTokenAsync(string email, string name)
        {
            throw new NotImplementedException();
        }

        public async Task<AuthResult> RegisterAsync(CreateUserCommand command)
        {
            var user = new ApplicationUser()
            {
                UserName = new MailAddress(command.Email).User,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Email = command.Email,
            };

            var result = await _userManager.CreateAsync(user, command.Password);

            if (!result.Succeeded) throw new UserCreationException(result.Errors.Select(e => e.Description));

            var claims = await _userManager.GetClaimsAsync(user);

            var token = _tokenService.CreateJwtToken(user, claims);

            return new AuthResult
            {
                Email = command.Email,
                Token = token,
                ExpiresOn = DateTime.UtcNow.AddHours(_options.Value.Duration),
                Username = user.UserName,
            };

        }

        public Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
