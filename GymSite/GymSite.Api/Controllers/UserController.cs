using GymSite.Api.Infrastructure;
using GymSite.Application.Abstractions;
using GymSite.Application.Commands;
using GymSite.Models.Response;
using GymSite.Models.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSite.Api.Controllers
{
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMediator _mediator;

        public UserController(IUserService userService, IMediator mediator)
        {
            _userService = userService;
            _mediator = mediator;
        }

        /// <summary>
        /// Returns info about user with given id
        /// </summary>
        /// <param name="id">User id</param>
        /// <response code="200">User model</response>
        /// <response code="404">User with given id does not exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<DataResponseModel<UserModel>>> GetUserById(string id)
        {
            var res = await _userService.GetUserById(id);

            return res.CreateOkOrNotFound();
        }

        /// <summary>
        /// Updates user with info specified in request
        /// </summary>
        /// <response code="204">User successfully updated</response>
        /// <response code="400">Error occured during update</response>
        [HttpPut]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ResponseModel>> UpdateUser(UpdateUserCommand command)
        {
            var res = await _mediator.Send(command);

            return res.CreateNoContentOrBadRequest();
        }
    }
}
