using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using TaskMate.Application.Dtos;
using TaskMate.Application.Exceptions;
using TaskMate.Application.Interfaces;
using TaskMate.Application.Options;
using TaskMate.Application.User.CreateUser;
using TaskMate.Domain.Entities;

namespace TaskMate.Application.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService, IOptions<JWTOptions> options) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ITokenService _tokenService = tokenService;
        private readonly IOptions<JWTOptions> _options = options;

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
            var refreshToken = _tokenService.GenerateRefreshToken();
            //we add it to the list of refresh tokens to this user
            user.RefreshTokens.Add(refreshToken);
            //update the user to presist the changes.
            await _userManager.UpdateAsync(user);
            return new AuthResult
            {
                Email = command.Email,
                Token = token,
                IsAuthenticated = true,
                ExpiresOn = DateTime.UtcNow.AddHours(_options.Value.Duration),
                Username = user.UserName,
                RefreshToken = token,
                RefreshTokenExpiresOn = refreshToken.ExpiresOn
            };

        }

        public async Task<AuthResult> GetTokenAsync(string email, string password)
        {
            //check for email
            var user = await _userManager.FindByEmailAsync(email) ?? throw new BadRequestException("Email or Password is incorrect");

            //check for password
            var result = await _userManager.CheckPasswordAsync(user, password);

            if (!result) throw new BadRequestException("Email or Password is incorrect");



            //if we get here that means the user exist

            //will full fill this and return it
            var authResult = new AuthResult();

            var userClaims = await _userManager.GetClaimsAsync(user);
            var token = _tokenService.CreateJwtToken(user, userClaims);


            if (user.RefreshTokens.Any(rf => rf.IsActive))
            {
                //if yes we select it
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(rf => rf.IsActive);
                authResult.RefreshToken = activeRefreshToken!.Token;
                authResult.RefreshTokenExpiresOn = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var newRefreshToken = _tokenService.GenerateRefreshToken();
                authResult.RefreshToken = newRefreshToken.Token;
                authResult.RefreshTokenExpiresOn = newRefreshToken.ExpiresOn;
                user.RefreshTokens.Add(newRefreshToken);
                await _userManager.UpdateAsync(user);
            }


            authResult.Username = user.UserName;
            authResult.Email = user.Email;
            authResult.Token = token;
            authResult.IsAuthenticated = true;
            authResult.ExpiresOn = DateTime.UtcNow.AddHours(_options.Value.Duration);
            return authResult;
        }

        public async Task<AuthResult> GetRefreshTokenAsync(string refreshToken)
        {
            var authModel = new AuthResult();

            //we try to get the user that has this refresh token if any
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(rf => rf.Token == refreshToken));

            if (user is null) throw new SecurityTokenException("Invalid refresh token");

            var refreshTokenFromDb = user.RefreshTokens.First(rf => rf.Token == refreshToken);

            if (!refreshTokenFromDb.IsActive) throw new SecurityTokenException("Inactive refresh token");

            //revoke the token
            refreshTokenFromDb.RevokedOn = DateTime.UtcNow;

            //2. create new refresh token and add it to this user and save it in the DB
            var newRefreshToken = _tokenService.GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            //3. create new JWT Token
            var claims = await _userManager.GetClaimsAsync(user);
            var jwtToken = _tokenService.CreateJwtToken(user, claims);


            // => finally initialize the return model (auth model)
            authModel.IsAuthenticated = true;
            authModel.ExpiresOn =  DateTime.UtcNow.AddHours(_options.Value.Duration);
            authModel.Email = user.Email;
            authModel.Token = jwtToken;
            authModel.Username = user.UserName;
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiresOn = newRefreshToken.ExpiresOn;

            return authModel;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string refreshToken)
        {
            //we try to get the user that has this refresh token if any
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));

            //if there is no user with this refresh token
            if (user is null) throw new SecurityTokenException("invalid refresh token");

            // if we get here that meant the refresh token exist
            var token = user.RefreshTokens.Single(rf=>rf.Token == refreshToken);

            //check if it's active or not first
            if(!token.IsActive) throw new SecurityTokenException("refresh Token is already inactive"); ;

            //here it means it is active
            token.RevokedOn = DateTime.UtcNow;

            //update user to presist the changes in the refresh token.
            await _userManager.UpdateAsync(user);

            return true;
        }

    }
}
