namespace GymSite.Database.User
{
    [Implementation(typeof(IUserInfoRepository))]
    public class UserInfoRepository : IUserInfoRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public UserInfoRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddInfoAsync(UserInfo userInfo)
        {
            _dbContext.UserInfo.AddAsync(userInfo);

            return _dbContext.SaveChangesAsync();
        }

        public Task<T?> GetInfoAsync<T>(int id, Func<UserInfo, T> selector)
            => Task.FromResult(_dbContext.UserInfo.Where(info => info.Id == id).Select(selector).FirstOrDefault());
    }
}
