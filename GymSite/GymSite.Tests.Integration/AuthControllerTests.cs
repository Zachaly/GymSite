using GymSite.Application.Commands;
using GymSite.Domain.Entity;
using GymSite.Models.Response;
using GymSite.Models.User.Request;
using GymSite.Models.User.Response;
using System.Net;
using System.Net.Http.Json;

namespace GymSite.Tests.Integration
{
    public class AuthControllerTests : IntegrationTest
    {
        [Fact]
        public async Task Register_User_Success()
        {
            var request = new AddUserRequest
            {
                Email = "test@email.com",
                FirstName = "fname",
                Gender = Domain.Enum.Gender.Male,
                LastName = "lname",
                NickName = "nickname",
                Password = "zaq1@WSX",
                Username = "username"
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(request.Username, GetFromDatabase<ApplicationUser>().Select(x => x.UserName));
        }

        [Fact]
        public async Task Register_User_Fail_InvalidRequest()
        {
            var request = new AddUserRequest
            {
                Gender = Domain.Enum.Gender.Male,
                LastName = "lname",
                NickName = "nickname",
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Login_Success()
        {
            var registerRequest = new AddUserRequest
            {
                Email = "test@email.com",
                FirstName = "fname",
                Gender = Domain.Enum.Gender.Male,
                LastName = "lname",
                NickName = "nickname",
                Password = "zaq1@WSX",
                Username = "username"
            };

            await _httpClient.PostAsJsonAsync("api/auth/register", registerRequest);

            var loginRequest = new LoginCommand
            {
                Password = registerRequest.Password,
                Username = registerRequest.Username,
            };

            var response = await  _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull((await response.Content.ReadFromJsonAsync<DataResponseModel<LoginResponse>>()).Data);
        }

        [Fact]
        public async Task Login_Fail()
        {
            var registerRequest = new AddUserRequest
            {
                Email = "test@email.com",
                FirstName = "fname",
                Gender = Domain.Enum.Gender.Male,
                LastName = "lname",
                NickName = "nickname",
                Password = "zaq1@WSX",
                Username = "username"
            };

            await _httpClient.PostAsJsonAsync("api/auth/register", registerRequest);

            var loginRequest = new LoginCommand
            {
                Password = "wrong password",
                Username = registerRequest.Username,
            };

            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);
            var responseContent = await response.Content.ReadFromJsonAsync<ResponseModel>();

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal("Username or password incorrect!", responseContent.Message);
        }
    }
}
