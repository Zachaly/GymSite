using GymSite.Database.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace GymSite.Database.Repository
{
    [Implementation(typeof(IArticleRepository))]
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ArticleRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task AddArticleAsync(Article article)
        {
            _dbContext.Article.Add(article);

            return _dbContext.SaveChangesAsync();
        }

        public Task DeleteArticleByIdAsync(int id)
        {
            var article = _dbContext.Article.Find(id);

            _dbContext.Article.Remove(article);

            return _dbContext.SaveChangesAsync();
        }

        public T GetArticleById<T>(int id, Func<Article, T> selector)
            => _dbContext.Article
                .Include(article => article.Creator)
                .Where(article => article.Id == id)
                .Select(selector)
                .FirstOrDefault();

        public IEnumerable<T> GetArticles<T>(int pageIndex, int pageSize, Func<Article, T> selector)
            => _dbContext.Article
                .Include(article => article.Creator)
                .OrderByDescending(article => article.Created)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(selector);

        public IEnumerable<T> GetUserArticles<T>(string userId, int pageIndex, int pageSize, Func<Article, T> selector)
            => _dbContext.Article
                .Include(article => article.Creator)
                .Where(article => article.CreatorId == userId)
                .OrderByDescending(article => article.Created)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(selector);
    }
}
