using GymSite.Application.Abstractions;
using GymSite.Models.Response;
using GymSite.Models.User.Request;
using MediatR;

namespace GymSite.Application.Commands
{
    public class UpdateUserCommand : UpdateUserRequest, IRequest<ResponseModel>
    {
    }

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, ResponseModel>
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UpdateUserHandler(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        public async Task<ResponseModel> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var id = await _authService.GetCurrentUserId();

            return await _userService.UpdateUser(request, id);
        }
    }
}
