namespace GymSite.Database.Repository.Abstractions
{
    public interface IUserInfoRepository
    {
        Task AddInfoAsync(UserInfo userInfo);
        Task<T?> GetInfoAsync<T>(int id, Func<UserInfo, T> selector);
    }
}
