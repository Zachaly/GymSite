using GymSite.Application;
using GymSite.Application.Abstractions;
using GymSite.Database.Repository.Abstractions;
using GymSite.Models.Response;
using GymSite.Models.Article;
using GymSite.Models.Article.Request;
using GymSite.Domain.Entity;
using Moq;
using Microsoft.EntityFrameworkCore;

namespace GymSite.Tests.Unit.Service
{
    [TestFixture]
    public class ArticleServiceTests
    {
        [Test]
        public async Task AddArticleAsync_Success()
        {
            var articles = new List<Article>();

            var articleFactoryMock = new Mock<IArticleFactory>();
            articleFactoryMock.Setup(x => x.Create(It.IsAny<AddArticleRequest>()))
                .Returns((AddArticleRequest request) => new Article { Title = request.Title });

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(""))
                .Returns(new ResponseModel { Success = true });

            var repositoryMock = new Mock<IArticleRepository>();
            repositoryMock.Setup(x => x.AddArticleAsync(It.IsAny<Article>()))
                .Callback((Article article) => articles.Add(article));

            var service = new ArticleService(repositoryMock.Object, articleFactoryMock.Object, responseFactoryMock.Object);

            var request = new AddArticleRequest
            {
                Content = new string('a', 100),
                CreatorId = "id",
                Description = "description",
                Title = "title",
            };

            var res = await service.AddArticleAsync(request);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(articles.Any(x => x.Title == request.Title));
            });
        }

        [Test]
        public async Task AddArticleAsync_InvalidRequest_Fail()
        {
            var articles = new List<Article>();

            var articleFactoryMock = new Mock<IArticleFactory>();
            articleFactoryMock.Setup(x => x.Create(It.IsAny<AddArticleRequest>()))
                .Returns((AddArticleRequest request) => new Article { Title = request.Title });

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateFail("", It.IsAny<Dictionary<string, IEnumerable<string>>>()))
                .Returns(new ResponseModel { Success = false });

            var repositoryMock = new Mock<IArticleRepository>();
            repositoryMock.Setup(x => x.AddArticleAsync(It.IsAny<Article>()))
                .Callback((Article article) => articles.Add(article));

            var service = new ArticleService(repositoryMock.Object, articleFactoryMock.Object, responseFactoryMock.Object);

            var request = new AddArticleRequest
            {
                CreatorId = "id",
                Description = "description",
                Title = "title",
            };

            var res = await service.AddArticleAsync(request);

            Assert.Multiple(() =>
            {
                Assert.That(!res.Success);
                Assert.That(!articles.Any(x => x.Title == request.Title));
            });
        }

        [Test]
        public async Task AddArticleAsync_RepositoryException_Fail()
        {
            var articles = new List<Article>();

            var articleFactoryMock = new Mock<IArticleFactory>();
            articleFactoryMock.Setup(x => x.Create(It.IsAny<AddArticleRequest>()))
                .Returns((AddArticleRequest request) => new Article { Title = request.Title });

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateFail("", It.IsAny<Dictionary<string, IEnumerable<string>>>()))
                .Returns(new ResponseModel { Success = false });

            var repositoryMock = new Mock<IArticleRepository>();
            repositoryMock.Setup(x => x.AddArticleAsync(It.IsAny<Article>()))
                .Callback((Article article) => throw new Exception());

            var service = new ArticleService(repositoryMock.Object, articleFactoryMock.Object, responseFactoryMock.Object);

            var request = new AddArticleRequest
            {
                CreatorId = "id",
                Description = "description",
                Title = "title",
            };

            var res = await service.AddArticleAsync(request);

            Assert.Multiple(() =>
            {
                Assert.That(!res.Success);
                Assert.That(!articles.Any(x => x.Title == request.Title));
            });
        }

        [Test]
        public async Task DeleteArticleById_Success()
        {
            var articles = new List<Article>
            {
                new Article { Id = 1 },
                new Article { Id = 2 },
                new Article { Id = 3 },
                new Article { Id = 4 },
                new Article { Id = 5 },
            };

            var articleFactoryMock = new Mock<IArticleFactory>();

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(""))
                .Returns(new ResponseModel { Success = true });

            var repositoryMock = new Mock<IArticleRepository>();
            repositoryMock.Setup(x => x.DeleteArticleByIdAsync(It.IsAny<int>()))
                .Callback((int id) => articles.Remove(articles.FirstOrDefault(x => x.Id == id)));

            var service = new ArticleService(repositoryMock.Object, articleFactoryMock.Object, responseFactoryMock.Object);

            const int Id = 3;

            var res = await service.DeleteArticleByIdAsync(Id);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(!articles.Any(x => x.Id == Id));
            });
        }

        [Test]
        public async Task DeleteArticleById_RepositoryError_Fail()
        {
            var articles = new List<Article>
            {
                new Article { Id = 1 },
                new Article { Id = 2 },
                new Article { Id = 3 },
                new Article { Id = 4 },
                new Article { Id = 5 },
            };

            var articleFactoryMock = new Mock<IArticleFactory>();

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateFail(It.IsAny<string>(), null))
                .Returns(new ResponseModel { Success = false });

            var repositoryMock = new Mock<IArticleRepository>();
            repositoryMock.Setup(x => x.DeleteArticleByIdAsync(It.IsAny<int>()))
                .Callback((int id) => throw new Exception());

            var service = new ArticleService(repositoryMock.Object, articleFactoryMock.Object, responseFactoryMock.Object);

            const int Id = 3;

            var res = await service.DeleteArticleByIdAsync(Id);

            Assert.That(!res.Success);
        }

        [Test]
        public async Task GetArticleById_Success()
        {
            var articles = new List<Article>
            {
                new Article { Id = 1, Title = "title1" },
                new Article { Id = 2, Title = "title2" },
                new Article { Id = 3, Title = "title3" },
                new Article { Id = 4, Title = "title4" },
                new Article { Id = 5, Title = "title5" },
            };

            var articleFactoryMock = new Mock<IArticleFactory>();
            articleFactoryMock.Setup(x => x.CreateModel(It.IsAny<Article>()))
                .Returns((Article article) => new ArticleModel { Id = article.Id, Title = article.Title });

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<ArticleModel>(), ""))
                .Returns((ArticleModel data, string _) => new DataResponseModel<ArticleModel> { Data = data, Success = true });

            var repositoryMock = new Mock<IArticleRepository>();
            repositoryMock.Setup(x => x.GetArticleById(It.IsAny<int>(), It.IsAny<Func<Article,ArticleModel>>()))
                .Returns((int id, Func<Article,ArticleModel> selector) => articles.Select(selector).FirstOrDefault(x => x.Id == id));

            var service = new ArticleService(repositoryMock.Object, articleFactoryMock.Object, responseFactoryMock.Object);

            const int Id = 3;

            var res = await service.GetArticleById(Id);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(res.Data.Title, Is.EqualTo(articles.FirstOrDefault(x => x.Id == Id).Title));
            });
        }

        [Test]
        public async Task GetArticleById_NotFound()
        {
            var articles = new List<Article>
            {
                new Article { Id = 1, Title = "title1" },
                new Article { Id = 2, Title = "title2" },
                new Article { Id = 3, Title = "title3" },
                new Article { Id = 4, Title = "title4" },
                new Article { Id = 5, Title = "title5" },
            };

            var articleFactoryMock = new Mock<IArticleFactory>();
            articleFactoryMock.Setup(x => x.CreateModel(It.IsAny<Article>()))
                .Returns((Article article) => new ArticleModel { Id = article.Id, Title = article.Title });

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateFail<ArticleModel>("", null))
                .Returns(new DataResponseModel<ArticleModel> { Data = null, Success = false });

            var repositoryMock = new Mock<IArticleRepository>();
            repositoryMock.Setup(x => x.GetArticleById(It.IsAny<int>(), It.IsAny<Func<Article, ArticleModel>>()))
                .Returns((int id, Func<Article, ArticleModel> selector) => articles.Select(selector).FirstOrDefault(x => x.Id == id));

            var service = new ArticleService(repositoryMock.Object, articleFactoryMock.Object, responseFactoryMock.Object);

            const int Id = 2137;

            var res = await service.GetArticleById(Id);

            Assert.Multiple(() =>
            {
                Assert.That(!res.Success);
                Assert.That(res.Data, Is.Null);
            });
        }

        [Test]
        public async Task GetArticles_CreatorIdNotSpecified()
        {
            var articles = new List<Article>
            {
                new Article { Id = 1, Title = "title1", CreatorId = "id" },
                new Article { Id = 2, Title = "title2", CreatorId = "id2" },
                new Article { Id = 3, Title = "title3", CreatorId = "id" },
                new Article { Id = 4, Title = "title4", CreatorId = "id2" },
                new Article { Id = 5, Title = "title5", CreatorId = "id" },
            };

            var articleFactoryMock = new Mock<IArticleFactory>();
            articleFactoryMock.Setup(x => x.CreateListItem(It.IsAny<Article>()))
                .Returns((Article article) => new ArticleListItemModel { Id = article.Id, Title = article.Title });

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<IEnumerable<ArticleListItemModel>>(), ""))
                .Returns((IEnumerable<ArticleListItemModel> data, string _) => new DataResponseModel<IEnumerable<ArticleListItemModel>>
                {
                    Data = data,
                    Success = true,
                });

            var repositoryMock = new Mock<IArticleRepository>();
            repositoryMock.Setup(x => x.GetArticles(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<Article, ArticleListItemModel>>()))
                .Returns((int index, int size, Func<Article, ArticleListItemModel> selector) 
                => articles.Select(selector).Skip(index * size).Take(size));

            var request = new GetArticleRequest { PageIndex = 0, PageSize = 2 };

            var service = new ArticleService(repositoryMock.Object, articleFactoryMock.Object, responseFactoryMock.Object);

            var res = await service.GetArticles(request);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(res.Data.Count(), Is.EqualTo(request.PageSize));
            });
        }

        [Test]
        public async Task GetArticles_CreatorIdSpecified()
        {
            var articles = new List<Article>
            {
                new Article { Id = 1, Title = "title1", CreatorId = "id" },
                new Article { Id = 2, Title = "title2", CreatorId = "id2" },
                new Article { Id = 3, Title = "title3", CreatorId = "id" },
                new Article { Id = 4, Title = "title4", CreatorId = "id2" },
                new Article { Id = 5, Title = "title5", CreatorId = "id" },
            };

            var articleFactoryMock = new Mock<IArticleFactory>();
            articleFactoryMock.Setup(x => x.CreateListItem(It.IsAny<Article>()))
                .Returns((Article article) => new ArticleListItemModel { Id = article.Id, Title = article.Title });

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<IEnumerable<ArticleListItemModel>>(), ""))
                .Returns((IEnumerable<ArticleListItemModel> data, string _) => new DataResponseModel<IEnumerable<ArticleListItemModel>>
                {
                    Data = data,
                    Success = true,
                });

            var repositoryMock = new Mock<IArticleRepository>();
            repositoryMock.Setup(x => x.GetUserArticles(It.IsAny<string>(),It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<Article, ArticleListItemModel>>()))
                .Returns((string userId, int index, int size, Func<Article, ArticleListItemModel> selector)
                => articles.Where(x => x.CreatorId == userId).Select(selector).Skip(index * size).Take(size));

            var request = new GetArticleRequest { PageIndex = 0, PageSize = 4, CreatorId = "id" };

            var service = new ArticleService(repositoryMock.Object, articleFactoryMock.Object, responseFactoryMock.Object);

            var res = await service.GetArticles(request);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(res.Data.Count(),
                    Is.EqualTo(articles.Where(x => x.CreatorId == request.CreatorId).Take(request.PageSize.Value).Count()));
            });
        }

        [Test]
        public async Task GetArticles_NullRequest()
        {
            var articles = new List<Article>
            {
                new Article { Id = 1, Title = "title1", CreatorId = "id" },
                new Article { Id = 2, Title = "title2", CreatorId = "id2" },
                new Article { Id = 3, Title = "title3", CreatorId = "id" },
                new Article { Id = 4, Title = "title4", CreatorId = "id2" },
                new Article { Id = 5, Title = "title5", CreatorId = "id" },
            };

            var articleFactoryMock = new Mock<IArticleFactory>();
            articleFactoryMock.Setup(x => x.CreateListItem(It.IsAny<Article>()))
                .Returns((Article article) => new ArticleListItemModel { Id = article.Id, Title = article.Title });

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<IEnumerable<ArticleListItemModel>>(), ""))
                .Returns((IEnumerable<ArticleListItemModel> data, string _) => new DataResponseModel<IEnumerable<ArticleListItemModel>>
                {
                    Data = data,
                    Success = true,
                });

            var repositoryMock = new Mock<IArticleRepository>();
            repositoryMock.Setup(x => x.GetArticles( It.IsAny<int>(), It.IsAny<int>(), It.IsAny<Func<Article, ArticleListItemModel>>()))
                .Returns((int index, int size, Func<Article, ArticleListItemModel> selector)
                => articles.Select(selector).Skip(index * size).Take(size));

            var request = new GetArticleRequest {  };

            var service = new ArticleService(repositoryMock.Object, articleFactoryMock.Object, responseFactoryMock.Object);

            var res = await service.GetArticles(request);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(res.Data.Count(),
                    Is.EqualTo(articles.Take(10).Count()));
            });
        }
    }
}
