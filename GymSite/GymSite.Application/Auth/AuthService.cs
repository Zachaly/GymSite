using GymSite.Application.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Domain.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GymSite.Application
{
    [Implementation(typeof(IAuthService))]
    public class AuthService : IAuthService
    {
        private readonly string _authAudience;
        private readonly string _authIssuer;
        private readonly string _secretKey;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(IConfiguration config, 
            UserManager<ApplicationUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _authAudience = config["Auth:Audience"];
            _authIssuer = config["Auth:Issuer"];
            _secretKey = config["Auth:SecretKey"];
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public Task<JwtSecurityToken> CreateToken(IEnumerable<Claim> claims)
        {
            var bytes = Encoding.UTF8.GetBytes(_secretKey);
            var key = new SymmetricSecurityKey(bytes);

            var algorithm = SecurityAlgorithms.HmacSha256;

            var credentials = new SigningCredentials(key, algorithm);

            var token = new JwtSecurityToken(
                _authAudience,
                _authIssuer,
                claims,
                DateTime.Now,
                DateTime.Now.AddHours(24),
                credentials);

            return Task.FromResult(token);
        }

        public Task<string> GetCurrentUserId()
            => Task.FromResult(_userManager.GetUserId(_httpContextAccessor.HttpContext.User));
    }
}
