using GymSite.Api.Infrastructure;
using GymSite.Application.Abstractions;
using GymSite.Models.Article;
using GymSite.Models.Article.Request;
using GymSite.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymSite.Api.Controllers
{
    [Route("api/article")]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// Get list of article list items specified by request
        /// </summary>
        /// <response code="200">List of articles</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<DataResponseModel<IEnumerable<ArticleListItemModel>>>> 
            GetArticles([FromQuery] GetArticleRequest getArticleRequest)
        {
            var res = await _articleService.GetArticles(getArticleRequest);

            return res.CreateOkOrBadRequest();
        }

        /// <summary>
        /// Get article model specified by id
        /// </summary>
        /// <response code="200">Article model</response>
        /// <response code="404">Article with specified id does not exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<DataResponseModel<ArticleModel>>> GetArticleById(int id)
        {
            var res = await _articleService.GetArticleById(id);

            return res.CreateOkOrNotFound();
        }

        /// <summary>
        /// Add article with data specified in request
        /// </summary>
        /// <response code="204">Article added successfully</response>
        /// <response code="404">Invalid data</response>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResponseModel>> AddArticleAsync(AddArticleRequest request)
        {
            var res = await _articleService.AddArticleAsync(request);

            return res.CreateNoContentOrBadRequest();
        }

        /// <summary>
        /// Add article with specified id
        /// </summary>
        /// <response code="204">Article deleted successfully</response>
        /// <response code="404">No article with specified id</response>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ResponseModel>> DeleteArticleByIdAsync(int id)
        {
            var res = await _articleService.DeleteArticleByIdAsync(id);

            return res.CreateNoContentOrBadRequest();
        }
    }
}
