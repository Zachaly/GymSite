namespace GymSite.Database.Repository.Abstractions
{
    public interface IArticleRepository
    {
        T GetArticleById<T>(int id, Func<Article, T> selector);
        Task AddArticleAsync(Article article);
        Task DeleteArticleByIdAsync(int id);
        IEnumerable<T> GetArticles<T>(int pageIndex, int pageSize, Func<Article, T> selector);
        IEnumerable<T> GetUserArticles<T>(string userId, int pageIndex, int pageSize, Func<Article, T> selector);
    }
}
