using Microsoft.Extensions.Options;
using System.Security.Claims;
using TaskMate.Application.Interfaces;
using TaskMate.Domain.Entities;
using TaskMate.Application.Options;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Cryptography;

namespace TaskMate.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly JWTOptions _options;

        public TokenService(IOptions<JWTOptions> options)
        {
            this._options = options.Value;
        }

        public string CreateJwtToken(ApplicationUser user, IList<Claim> userClaims)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("uid", user.Id),
            }.Union(userClaims);


            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key));
            var creds = new SigningCredentials(symmetricKey,SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(_options.Duration),
                signingCredentials : creds
                );

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public string GenerateRefreshToken()
        {
            var random = new byte[32];
            RandomNumberGenerator.Fill(random);

            return Convert.ToBase64String(random);
        }
    }
}
