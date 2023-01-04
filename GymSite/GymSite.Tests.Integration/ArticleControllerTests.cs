using GymSite.Domain.Entity;
using GymSite.Models.Article;
using GymSite.Models.Article.Request;
using GymSite.Models.Response;
using System.Net.Http.Json;

namespace GymSite.Tests.Integration
{
    public class ArticleControllerTests : IntegrationTest
    {
        [Theory]
        [InlineData(1, 5)]
        [InlineData(2, 3)]
        [InlineData(null, 5)]
        [InlineData(null, null)]
        public async Task GetArticle_OnlyIndexAndPageSize_Success(int? index, int? size)
        {
            var user = new ApplicationUser { Id = "id", NickName = "nickname" };
            await AddToDatabase(new List<ApplicationUser> { user });

            var testContent = new string('a', 500);

            var articles = new List<Article>
            {
                new Article { Content = testContent, Title = "title1", CreatorId = user.Id, Created = DateTime.Now.AddDays(-1) },
                new Article { Content = testContent, Title = "title2", CreatorId = user.Id, Created = DateTime.Now.AddDays(1) },
                new Article { Content = testContent, Title = "title3", CreatorId = user.Id, Created = DateTime.Now.AddDays(-2) },
                new Article { Content = testContent, Title = "title4", CreatorId = user.Id, Created = DateTime.Now.AddDays(2) },
                new Article { Content = testContent, Title = "title5", CreatorId = user.Id, Created = DateTime.Now.AddDays(-3) },
                new Article { Content = testContent, Title = "title6", CreatorId = user.Id, Created = DateTime.Now.AddDays(3) },
                new Article { Content = testContent, Title = "title7", CreatorId = user.Id, Created = DateTime.Now.AddDays(-4) },
                new Article { Content = testContent, Title = "title8", CreatorId = user.Id, Created = DateTime.Now.AddDays(4) },
                new Article { Content = testContent, Title = "title9", CreatorId = user.Id, Created = DateTime.Now.AddDays(-5) },
                new Article { Content = testContent, Title = "title10", CreatorId = user.Id, Created = DateTime.Now.AddDays(5) },
                new Article { Content = testContent, Title = "title11", CreatorId = user.Id, Created = DateTime.Now.AddDays(-6) },
                new Article { Content = testContent, Title = "title12", CreatorId = user.Id, Created = DateTime.Now.AddDays(6) },
            };

            await AddToDatabase(articles);

            var request = new GetArticleRequest
            {
                PageSize = size,
                PageIndex = index
            };

            var response = await _httpClient.GetAsync($"api/article?pageSize={request.PageSize}&pageIndex={request.PageIndex}");
            var content = await response.Content.ReadFromJsonAsync<DataResponseModel<IEnumerable<ArticleListItemModel>>>();

            var testArticles = GetFromDatabase<Article>()
                .OrderByDescending(x => x.Created)
                .Skip((request.PageSize ?? 10) * (request.PageIndex ?? 0))
                .Take(request.PageSize ?? 10);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(content.Success);
            Assert.True(content.Data.All(x => testArticles.Any(y => y.Title == x.Title)));
        }

        [Theory]
        [InlineData(1, 5, "id")]
        [InlineData(2, 3, "id")]
        [InlineData(1, 5, "id2")]
        [InlineData(2, 3, "id2")]
        [InlineData(1, 5, "id3")]
        public async Task GetArticle_FullRequest_Success(int? index, int? size, string? userId)
        {
            var user = new ApplicationUser { Id = "id", NickName = "nickname" };
            var user2 = new ApplicationUser { Id = "id2", NickName = "nickname" };
            await AddToDatabase(new List<ApplicationUser> { user, user2 });

            var testContent = new string('a', 500);

            var articles = new List<Article>
            {
                new Article { Content = testContent, Title = "title1", CreatorId = user.Id, Created = DateTime.Now.AddDays(-1) },
                new Article { Content = testContent, Title = "title2", CreatorId = user2.Id, Created = DateTime.Now.AddDays(1) },
                new Article { Content = testContent, Title = "title3", CreatorId = user.Id, Created = DateTime.Now.AddDays(-2) },
                new Article { Content = testContent, Title = "title4", CreatorId = user2.Id, Created = DateTime.Now.AddDays(2) },
                new Article { Content = testContent, Title = "title5", CreatorId = user.Id, Created = DateTime.Now.AddDays(-3) },
                new Article { Content = testContent, Title = "title6", CreatorId = user.Id, Created = DateTime.Now.AddDays(3) },
                new Article { Content = testContent, Title = "title7", CreatorId = user.Id, Created = DateTime.Now.AddDays(-4) },
                new Article { Content = testContent, Title = "title8", CreatorId = user2.Id, Created = DateTime.Now.AddDays(4) },
                new Article { Content = testContent, Title = "title9", CreatorId = user2.Id, Created = DateTime.Now.AddDays(-5) },
                new Article { Content = testContent, Title = "title10", CreatorId = user.Id, Created = DateTime.Now.AddDays(5) },
                new Article { Content = testContent, Title = "title11", CreatorId = user2.Id, Created = DateTime.Now.AddDays(-6) },
                new Article { Content = testContent, Title = "title12", CreatorId = user.Id, Created = DateTime.Now.AddDays(6) },
            };

            await AddToDatabase(articles);

            var request = new GetArticleRequest
            {
                PageSize = size,
                PageIndex = index,
                CreatorId = userId,
            };

            var response = await _httpClient.GetAsync($"api/article?pageSize={request.PageSize}&pageIndex={request.PageIndex}&creatorId={request.CreatorId}");
            var content = await response.Content.ReadFromJsonAsync<DataResponseModel<IEnumerable<ArticleListItemModel>>>();

            var testArticles = GetFromDatabase<Article>()
                .OrderByDescending(x => x.Created)
                .Where(x => x.CreatorId == request.CreatorId)
                .Skip((request.PageSize ?? 10) * (request.PageIndex ?? 0))
                .Take(request.PageSize ?? 10);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(content.Success);
            Assert.True(content.Data.All(x => testArticles.Any(y => y.Title == x.Title)));
        }

        [Fact]
        public async Task AddArticleAsync_ValidRequest_Success()
        {
            await Authenticate();

            var request = new AddArticleRequest
            {
                Content = new string('a', 500),
                Title = "title",
                CreatorId = GetAuthenticatedUser().Id,
                Description = "description",
            };

            var response = await _httpClient.PostAsJsonAsync("api/article", request);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(GetFromDatabase<Article>(), x => x.Title == request.Title && 
                x.Content == request.Content && 
                x.CreatorId == request.CreatorId && 
                x.Description == request.Description);
        }

        [Fact]
        public async Task AddArticleAsync_InvalidRequest_BadRequest()
        {
            await Authenticate();

            var request = new AddArticleRequest
            {
            };

            var response = await _httpClient.PostAsJsonAsync("api/article", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.DoesNotContain(GetFromDatabase<Article>(), x => x.Title == request.Title &&
                x.Content == request.Content &&
                x.CreatorId == request.CreatorId &&
                x.Description == request.Description);
        }

        [Fact]
        public async Task DeleteArticleById_Success()
        {
            await Authenticate();

            var user = new ApplicationUser { Id = "id", NickName = "nickname" };
            await AddToDatabase(new List<ApplicationUser> { user });

            var testContent = new string('a', 500);

            var articles = new List<Article>
            {
                new Article { Id = 1, Content = testContent, Title = "title1", CreatorId = user.Id },
                new Article { Id = 2, Content = testContent, Title = "title2", CreatorId = user.Id },
                new Article { Id = 3, Content = testContent, Title = "title3", CreatorId = user.Id },
                new Article { Id = 4, Content = testContent, Title = "title4", CreatorId = user.Id },
                new Article { Id = 5, Content = testContent, Title = "title5", CreatorId = user.Id },
            };

            await AddToDatabase(articles);

            const int Id = 3;

            var response = await _httpClient.DeleteAsync($"api/article/{Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(GetFromDatabase<Article>(), x => x.Id == Id);
        }

        [Fact]
        public async Task DeleteArticleById_BadRequest()
        {
            await Authenticate();

            var user = new ApplicationUser { Id = "id", NickName = "nickname" };
            await AddToDatabase(new List<ApplicationUser> { user });

            var testContent = new string('a', 500);

            var articles = new List<Article>
            {
                new Article { Id = 1, Content = testContent, Title = "title1", CreatorId = user.Id },
                new Article { Id = 2, Content = testContent, Title = "title2", CreatorId = user.Id },
                new Article { Id = 3, Content = testContent, Title = "title3", CreatorId = user.Id },
                new Article { Id = 4, Content = testContent, Title = "title4", CreatorId = user.Id },
                new Article { Id = 5, Content = testContent, Title = "title5", CreatorId = user.Id },
            };

            await AddToDatabase(articles);

            const int Id = 2137;

            var response = await _httpClient.DeleteAsync($"api/article/{Id}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.DoesNotContain(GetFromDatabase<Article>(), x => x.Id == Id);
        }

        [Fact]
        public async Task GetArticleById_Success()
        {
            var user = new ApplicationUser { Id = "id", NickName = "nickname" };
            await AddToDatabase(new List<ApplicationUser> { user });

            var testContent = new string('a', 500);

            var articles = new List<Article>
            {
                new Article { Id = 1, Content = testContent, Title = "title1", CreatorId = user.Id },
                new Article { Id = 2, Content = testContent, Title = "title2", CreatorId = user.Id },
                new Article { Id = 3, Content = testContent, Title = "title3", CreatorId = user.Id },
                new Article { Id = 4, Content = testContent, Title = "title4", CreatorId = user.Id },
                new Article { Id = 5, Content = testContent, Title = "title5", CreatorId = user.Id },
            };

            await AddToDatabase(articles);

            const int Id = 3;

            var response = await _httpClient.GetAsync($"api/article/{Id}");
            var content = await response.Content.ReadFromJsonAsync<DataResponseModel<ArticleModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(content.Data.Title, articles.FirstOrDefault(x => x.Id == Id).Title);
        }

        [Fact]
        public async Task GetArticleById_NotFound()
        {
            var user = new ApplicationUser { Id = "id", NickName = "nickname" };
            await AddToDatabase(new List<ApplicationUser> { user });

            var testContent = new string('a', 500);

            var articles = new List<Article>
            {
                new Article { Id = 1, Content = testContent, Title = "title1", CreatorId = user.Id },
                new Article { Id = 2, Content = testContent, Title = "title2", CreatorId = user.Id },
                new Article { Id = 3, Content = testContent, Title = "title3", CreatorId = user.Id },
                new Article { Id = 4, Content = testContent, Title = "title4", CreatorId = user.Id },
                new Article { Id = 5, Content = testContent, Title = "title5", CreatorId = user.Id },
            };

            await AddToDatabase(articles);

            const int Id = 2137;

            var response = await _httpClient.GetAsync($"api/article/{Id}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
