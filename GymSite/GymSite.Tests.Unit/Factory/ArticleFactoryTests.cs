using GymSite.Application;
using GymSite.Domain.Entity;
using GymSite.Models.Article.Request;

namespace GymSite.Tests.Unit.Factory
{
    [TestFixture]
    public class ArticleFactoryTests
    {
        [Test]
        public void Create()
        {
            var factory = new ArticleFactory();

            var request = new AddArticleRequest
            {
                Content = "content",
                CreatorId = "id",
                Description = "description",
                Title = "title",
            };

            var article = factory.Create(request);

            Assert.Multiple(() =>
            {
                Assert.That(article.Title, Is.EqualTo(request.Title));
                Assert.That(article.CreatorId, Is.EqualTo(request.CreatorId));
                Assert.That(article.Description, Is.EqualTo(request.Description));
                Assert.That(article.Content, Is.EqualTo(request.Content));
            });
        }

        [Test]
        public void CreateModel()
        {
            var factory = new ArticleFactory();

            var article = new Article
            {
                Content = "content",
                Created = DateTime.Now,
                Creator = new ApplicationUser { Id = "id", NickName = "nick" },
                CreatorId = "id",
                Description = "description",
                Id = 1,
                Title = "title",
            };

            var model = factory.CreateModel(article);

            Assert.Multiple(() =>
            {
                Assert.That(model.Id, Is.EqualTo(article.Id));
                Assert.That(model.Content, Is.EqualTo(article.Content));
                Assert.That(model.Title, Is.EqualTo(article.Title));
                Assert.That(model.Created, Is.EqualTo(article.Created.ToString("dd.MM.yyyy")));
                Assert.That(model.CreatorId, Is.EqualTo(article.CreatorId));
                Assert.That(model.CreatorName, Is.EqualTo(article.Creator.NickName));
            });
        }

        [Test]
        public void CreateListItemModel()
        {
            var factory = new ArticleFactory();

            var article = new Article
            {
                Content = "content",
                Created = DateTime.Now,
                Creator = new ApplicationUser { Id = "id", NickName = "nick" },
                CreatorId = "id",
                Description = "description",
                Id = 1,
                Title = "title",
            };

            var item = factory.CreateListItem(article);

            Assert.Multiple(() =>
            {
                Assert.That(item.Id, Is.EqualTo(article.Id));
                Assert.That(item.Title, Is.EqualTo(article.Title));
                Assert.That(item.CreatorName, Is.EqualTo(article.Creator.NickName));
                Assert.That(item.Description, Is.EqualTo(article.Description));
            });
        }
    }
}
