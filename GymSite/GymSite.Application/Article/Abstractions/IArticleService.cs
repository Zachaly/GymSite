using GymSite.Models.Article;
using GymSite.Models.Article.Request;
using GymSite.Models.Response;

namespace GymSite.Application.Abstractions
{
    public interface IArticleService
    {
        Task<ResponseModel> AddArticleAsync(AddArticleRequest request);
        Task<ResponseModel> DeleteArticleByIdAsync(int id);
        Task<DataResponseModel<ArticleModel>> GetArticleById(int id);
        Task<DataResponseModel<IEnumerable<ArticleListItemModel>>> GetArticles(GetArticleRequest request);
    }
}
