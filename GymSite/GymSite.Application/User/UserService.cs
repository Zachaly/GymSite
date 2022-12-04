using GymSite.Application.Response.Abstractions;
using GymSite.Application.User.Abstractions;
using GymSite.Database.User;
using GymSite.Domain.Entity;
using GymSite.Domain.Utils;
using GymSite.Models.Response;
using GymSite.Models.User.Request;
using Microsoft.AspNetCore.Identity;

namespace GymSite.Application.User
{
    [Implementation(typeof(IUserService))]
    public class UserService : IUserService
    {
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserFactory _userFactory;
        private readonly IResponseFactory _responseFactory;

        public UserService(IUserInfoRepository userInfoRepository,
            UserManager<ApplicationUser> userManager,
            IUserFactory userFactory,
            IResponseFactory responseFactory)
        {
            _userInfoRepository = userInfoRepository;
            _userManager = userManager;
            _userFactory = userFactory;
            _responseFactory = responseFactory;
        }

        public async Task<ResponseModel> AddUser(AddUserRequest request)
        {
            try
            {
                var user = _userFactory.Create(request);
                var userInfo = _userFactory.CreateInfo(request);

                await _userManager.CreateAsync(user, request.Password);

                userInfo.UserId = user.Id;

                await _userInfoRepository.AddInfoAsync(userInfo);

                user.UserInfoId = userInfo.Id;

                await _userManager.UpdateAsync(user);

                return _responseFactory.CreateSuccess(ResponseCode.NoContent, "User created");
            }
            catch(Exception ex)
            {
                return _responseFactory.CreateFail(ResponseCode.BadRequest, ex.Message, null);
            }
        }
    }
}
