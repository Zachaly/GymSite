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

        public Task<T?> GetUserByNameAsync<T>(string username, Func<ApplicationUser, T> selector)
            => Task.FromResult(_dbContext.Users.Where(user => user.UserName== username).Select(selector).FirstOrDefault());
    }
}
