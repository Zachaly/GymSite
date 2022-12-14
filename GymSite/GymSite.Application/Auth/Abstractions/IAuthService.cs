using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GymSite.Application.Abstractions
{
    public interface IAuthService
    {
        Task<string> GetCurrentUserId();
        Task<JwtSecurityToken> CreateToken(IEnumerable<Claim> claims);
    }
}
