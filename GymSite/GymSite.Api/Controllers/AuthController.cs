using GymSite.Api.Infrastructure;
using GymSite.Application.Abstractions;
using GymSite.Application.Commands;
using GymSite.Models.Response;
using GymSite.Models.User.Request;
using GymSite.Models.User.Response;
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

        /// <summary>
        /// Creates new user with data specified in request
        /// </summary>
        /// <response code="200">User added successfully</response>
        /// <response code="404">Failed to add user</response>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResponseModel>> Register(AddUserRequest request)
        {
            var res = await _userService.AddUser(request);

            return res.CreateOkOrBadRequest();
        }

        /// <summary>
        /// Returns jwt and user authorization info
        /// </summary>
        /// <response code="200">User authorized successfully</response>
        /// <response code="404">Login data is not valid</response>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<DataResponseModel<LoginResponse>>> Login(LoginCommand command)
        {
            var res = await _mediator.Send(command);

            return res.CreateOkOrBadRequest();
        }
    }
}
