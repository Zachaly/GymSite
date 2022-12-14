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

        [HttpGet("{id}")]
        public async Task<ActionResult<DataResponseModel<UserModel>>> GetUserById(string id)
        {
            var res = await _userService.GetUserById(id);

            return res.CreateOkOrNotFound();
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<ResponseModel>> UpdateUser(UpdateUserCommand command)
        {
            var res = await _mediator.Send(command);

            return res.CreateNoContentOrBadRequest();
        }
    }
}
