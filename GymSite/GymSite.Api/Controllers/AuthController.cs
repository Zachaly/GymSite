using GymSite.Api.Infrastructure;
using GymSite.Application.Auth.Abstractions;
using GymSite.Application.Auth.Commands;
using GymSite.Application.User.Abstractions;
using GymSite.Models.Response;
using GymSite.Models.User.Request;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace GymSite.Api.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMediator _mediator;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IMediator mediator, IUserService userService)
        {
            _authService = authService;
            _mediator = mediator;
            _userService = userService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<ResponseModel>> Register(AddUserRequest request)
        {
            var res = await _userService.AddUser(request);

            return res.CreateOkOrBadRequest();
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<ResponseModel>> Login(LoginCommand command)
        {
            var res = await _mediator.Send(command);

            return res.CreateOkOrBadRequest();
        }
    }
}
