using GymSite.Application.Abstractions;
using GymSite.Database.Repository.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Models.Article;
using GymSite.Models.Article.Request;
using GymSite.Models.Article.Validator;
using GymSite.Models.Response;
using GymSite.Domain.Utils;

namespace GymSite.Application
{
    [Implementation(typeof(IArticleService))]
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleFactory _articleFactory;
        private readonly IResponseFactory _responseFactory;

        public ArticleService(IArticleRepository articleRepository,IArticleFactory articleFactory, IResponseFactory responseFactory)
        {
            _articleFactory = articleFactory;
            _responseFactory = responseFactory;
            _articleRepository = articleRepository;
        }

        public async Task<ResponseModel> AddArticleAsync(AddArticleRequest request)
        {
            var validation = new AddArticleValidator().Validate(request);

            if(!validation.IsValid)
            {
                return _responseFactory.CreateFail("", validation.GetValidationErrors());
            }

            try
            {
                var article = _articleFactory.Create(request);

                await _articleRepository.AddArticleAsync(article);

                return _responseFactory.CreateSuccess();
            } 
            catch (Exception ex)
            {
                return _responseFactory.CreateFail(ex.Message, null);
            }
        }

        public async Task<ResponseModel> DeleteArticleByIdAsync(int id)
        {
            try
            {
                await _articleRepository.DeleteArticleByIdAsync(id);

                return _responseFactory.CreateSuccess();
            } 
            catch (Exception ex)
            {
                return _responseFactory.CreateFail(ex.Message, null);
            }
        }

        public async Task<DataResponseModel<ArticleModel>> GetArticleById(int id)
        {
            var data = _articleRepository.GetArticleById(id, article => _articleFactory.CreateModel(article));
            
            if(data is null)
            {
                return _responseFactory.CreateFail<ArticleModel>("", null);
            }

            return _responseFactory.CreateSuccess(data);
        }

        public Task<DataResponseModel<IEnumerable<ArticleListItemModel>>> GetArticles(GetArticleRequest request)
        {
            IEnumerable<ArticleListItemModel> data;

            var index = request.PageIndex ?? 0;
            var size = request.PageSize ?? 10;

            var selector = (Article article) => _articleFactory.CreateListItem(article);

            if(request.CreatorId is not null)
            {
                data = _articleRepository.GetUserArticles(request.CreatorId, index, size, selector);
            } else
            {
                data = _articleRepository.GetArticles(index, size, selector);
            }

            return Task.FromResult(_responseFactory.CreateSuccess(data));
        }
    }
}
