using GymSite.Application.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Domain.Utils;
using GymSite.Models.Article;
using GymSite.Models.Article.Request;

namespace GymSite.Application
{
    [Implementation(typeof(IArticleFactory))]
    public class ArticleFactory : IArticleFactory
    {
        public Article Create(AddArticleRequest request)
            => new Article
            {
                Content = request.Content,
                Created = DateTime.Now,
                CreatorId = request.CreatorId,
                Description = request.Description,
                Title = request.Title,
            };

        public ArticleListItemModel CreateListItem(Article article)
            => new ArticleListItemModel
            {
                CreatorName = article.Creator.NickName,
                Description = article.Description,
                Id = article.Id,
                Title = article.Title,
            };

        public ArticleModel CreateModel(Article article)
            => new ArticleModel
            {
                CreatorId = article.CreatorId,
                Content = article.Content,
                Created = article.Created.ToString("dd.MM.yyyy"),
                CreatorName = article.Creator.NickName,
                Id = article.Id,
                Title = article.Title,
            };
    }
}
