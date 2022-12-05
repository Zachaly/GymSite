using Microsoft.EntityFrameworkCore;

namespace GymSite.Database.User
{
    [Implementation(typeof(IUserRepository))]
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<T> GetUserByIdAsync<T>(string id, Func<ApplicationUser, T> selector)
            => Task.FromResult(_dbContext.Users
                .Include(user => user.UserInfo)
                .Where(user => user.Id == id)
                .Select(selector)
                .FirstOrDefault());

        public Task<T> GetUserByNameAsync<T>(string username, Func<ApplicationUser, T> selector)
            => Task.FromResult(_dbContext.Users.Where(user => user.UserName == username).Select(selector).FirstOrDefault());

        public Task UpdateUser(ApplicationUser user)
        {
            _dbContext.Users.Update(user);
            
            return Task.CompletedTask;
        }
    }
}
