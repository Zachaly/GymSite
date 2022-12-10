using GymSite.Domain.Entity;
using GymSite.Models.Response;
using GymSite.Models.User.Request;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GymSite.Application.Auth.Abstractions
{
    public interface IAuthService
    {
        Task<string> GetCurrentUserId();
        Task<JwtSecurityToken> CreateToken(IEnumerable<Claim> claims);
    }
}
