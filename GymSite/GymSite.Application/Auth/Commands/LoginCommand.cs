using GymSite.Application.Auth.Abstractions;
using GymSite.Application.Response.Abstractions;
using GymSite.Database.User;
using GymSite.Domain.Entity;
using GymSite.Models.Response;
using GymSite.Models.User.Response;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace GymSite.Application.Auth.Commands
{
    public class LoginCommand : IRequest<DataResponseModel<LoginResponse>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class LoginHandler : IRequestHandler<LoginCommand, DataResponseModel<LoginResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IResponseFactory _responseFactory;
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;

        public LoginHandler(UserManager<ApplicationUser> userManager,
            IResponseFactory responseFactory,
            IAuthService authService,
            IUserRepository userRepository)
        {
            _userManager = userManager;
            _responseFactory = responseFactory;
            _authService = authService;
            _userRepository = userRepository;
        }

        public async Task<DataResponseModel<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByNameAsync(request.Username, x => x);

            if (user is null)
            {
                return CreateFail();
            }

            if (!await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return CreateFail();
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
            };

            claims.AddRange(_userManager.GetClaimsAsync(user).Result);

            var data = new LoginResponse
            {
                AuthToken = new JwtSecurityTokenHandler().WriteToken(await _authService.CreateToken(claims)),
                UserId = user.Id,
                Username = user.UserName,
                Claims = GetRoleClaims(claims)
            };

            return _responseFactory.CreateSuccess(ResponseCode.Ok, "", data);
        }

        private DataResponseModel<LoginResponse> CreateFail()
            => _responseFactory.CreateFail<LoginResponse>(ResponseCode.BadRequest, "Username or password incorrect!", null);

        private IEnumerable<string> GetRoleClaims(IEnumerable<Claim> claims)
            => claims.Where(claim => claim.Type == "Role").Select(claim => claim.Value);
    }
}
