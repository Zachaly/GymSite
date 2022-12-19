using GymSite.Application.Commands;
using GymSite.Database;
using GymSite.Domain.Entity;
using GymSite.Models.Response;
using GymSite.Models.User.Request;
using GymSite.Models.User.Response;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Headers;
using System.Net.Http.Json;
// needed to disaple parallel test bsc db was going nuts with random names
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace GymSite.Tests.Integration
{
    public class IntegrationTest : IDisposable
    {
        protected readonly HttpClient _httpClient;
        protected readonly WebApplicationFactory<Program> _webFactory;
        private readonly string _authUsername = "authorized";

        public IntegrationTest()
        {
            _webFactory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll(typeof(DbContextOptions<ApplicationDbContext>));
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb");
                    });
                    // app will not ask you to add admin if one does not exist
                    Console.SetIn(new StringReader("no"));
                });
            });

            using(var scope = _webFactory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
            }

            _httpClient = _webFactory.CreateClient();
        }

        protected async Task Authenticate()
        {
            var addUserRequest = new AddUserRequest
            {
                NickName = "authorize",
                Email = "auth@email.com",
                Password = "zaq1@WSX",
                Username = _authUsername,
                FirstName = "Test",
                Gender = 0,
                LastName = "Test",
            };

            await _httpClient.PostAsJsonAsync("api/auth/register", addUserRequest);

            var loginCommand = new LoginCommand
            {
                Password = addUserRequest.Password,
                Username = addUserRequest.Username,
            };

            var token = await (await _httpClient.PostAsJsonAsync("api/auth/login", loginCommand))
                .Content.ReadFromJsonAsync<DataResponseModel<LoginResponse>>();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue($"Bearer", token.Data.AuthToken);
        }

        protected ApplicationUser GetAuthenticatedUser()
            => GetFromDatabase<ApplicationUser>().FirstOrDefault(x => x.UserName == _authUsername);

        protected async Task AddToDatabase<T>(List<T> items) where T : class
        {
            var scope = _webFactory.Services.CreateScope();

            using (scope)
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                await dbContext.Set<T>().AddRangeAsync(items);
                await dbContext.SaveChangesAsync();
            }
        }

        protected IEnumerable<T> GetFromDatabase<T>() where T : class 
        {
            var scope = _webFactory.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            return dbContext.Set<T>().AsEnumerable();
        }

        public void Dispose()
        {
            using(var scope = _webFactory.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetService<ApplicationDbContext>();
                db.Database.EnsureDeleted();
            }
        }
    }
}
