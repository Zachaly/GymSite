using GymSite.Domain.Entity;
using GymSite.Models.Article;
using GymSite.Models.Article.Request;

namespace GymSite.Application.Abstractions
{
    public interface IArticleFactory
    {
        Article Create(AddArticleRequest request);
        ArticleModel CreateModel(Article article);
        ArticleListItemModel CreateListItem(Article article);
    }
}
