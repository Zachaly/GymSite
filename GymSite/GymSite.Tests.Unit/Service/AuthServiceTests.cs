using GymSite.Application.Auth;
using GymSite.Domain.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GymSite.Tests.Unit.Service
{
    public class AuthServiceTests
    {
        private static Mock<IConfiguration> MockConfig()
        {
            var config = new Mock<IConfiguration>();
            config.SetupGet(x => x[It.Is<string>(s => s == "Auth:Audience")]).Returns("https://localhost");
            config.SetupGet(x => x[It.Is<string>(s => s == "Auth:Issuer")]).Returns("https://localhost");
            config.SetupGet(x => x[It.Is<string>(s => s == "Auth:SecretKey")]).Returns("supersecretkeyloooooooooooooooooooooooooooooooong");
            return config;
        }

        private static Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            var mgr = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            mgr.Object.UserValidators.Add(new UserValidator<ApplicationUser>());
            mgr.Object.PasswordValidators.Add(new PasswordValidator<ApplicationUser>());

            return mgr;
        }

        [Test]
        public async Task GetCurrentUserId()
        {
            const string UserId = "id";

            var userManagerMock = MockUserManager();
            var configMock = new Mock<IConfiguration>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            
            var userPrincipal = new ClaimsPrincipal();
            userPrincipal.AddIdentity(new ClaimsIdentity(new List<Claim> { new Claim(JwtRegisteredClaimNames.Sub, UserId) }));
            httpContextAccessorMock.Setup(x => x.HttpContext.User).Returns(userPrincipal);
            userManagerMock
                .Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns((ClaimsPrincipal principal) 
                    => principal.Identities.First().Claims.First(x => x.Type == JwtRegisteredClaimNames.Sub).Value);

            var service = new AuthService(configMock.Object, userManagerMock.Object, httpContextAccessorMock.Object);

            var id = await service.GetCurrentUserId();

            Assert.That(id, Is.EqualTo(UserId));
        }

        [Test]
        public async Task CreateToken()
        {
            var userManagerMock = MockUserManager();
            var configMock = MockConfig();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var service = new AuthService(configMock.Object, userManagerMock.Object, httpContextAccessorMock.Object);

            var idClaim = new Claim(JwtRegisteredClaimNames.Sub, "id");
            var nameClaim = new Claim(JwtRegisteredClaimNames.UniqueName, "name");

            var claims = new List<Claim> { idClaim, nameClaim };

            var token = await service.CreateToken(claims);

            var tokenValidation = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configMock.Object["Auth:SecretKey"])),
                ValidIssuer = configMock.Object["Auth:Issuer"],
                ValidAudience = configMock.Object["Auth:Audience"]
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var res = tokenHandler.ValidateToken(tokenHandler.WriteToken(token), tokenValidation, out SecurityToken validToken);

            Assert.Multiple(() =>
            {
                Assert.That(res.Claims.Any(x => x.Value == idClaim.Value));
                Assert.That(res.Claims.Any(x => x.Value == nameClaim.Value));
            });
        }
    }
}
