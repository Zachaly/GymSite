using GymSite.Domain.Entity;

namespace GymSite.Database.User
{
    public interface IUserInfoRepository
    {
        Task AddInfoAsync(UserInfo userInfo);
        Task<T?> GetInfoAsync<T>(int id, Func<UserInfo, T> selector);
    }
}
