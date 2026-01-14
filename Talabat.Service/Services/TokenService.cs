using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Talabat.Core.Models.Identity;
using Talabat.Core.Services;

namespace Talabat.Service.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> CreateTokenAsync(AppUser User, UserManager<AppUser> userManager)
        {

            // Payloads => private claims
            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, User.Id),
                new Claim(ClaimTypes.GivenName, User.DisplayName),
                new Claim(ClaimTypes.Email,User.Email),
            };
            var UserRoles = await userManager.GetRolesAsync(User);
            foreach (var role in UserRoles)
                Claims.Add(new Claim(ClaimTypes.Role, role));
            // key
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]!));

            var Token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                claims: Claims,
                expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInMinutes"]!)),
                signingCredentials: new SigningCredentials(Key, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(Token);
        }
    }
}
