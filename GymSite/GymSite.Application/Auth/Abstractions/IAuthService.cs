using GymSite.Domain.Entity;
using GymSite.Models.Response;
using GymSite.Models.User.Request;
using System.IdentityModel.Tokens.Jwt;

namespace GymSite.Application.Auth.Abstractions
{
    public interface IAuthService
    {
        Task<string> GetCurrentUserId();
        Task<JwtSecurityToken> CreateToken(ApplicationUser user);
    }
}
