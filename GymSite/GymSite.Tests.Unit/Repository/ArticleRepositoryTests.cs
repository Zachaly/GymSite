using GymSite.Database.Repository;
using GymSite.Domain.Entity;

namespace GymSite.Tests.Unit.Repository
{
    public class ArticleRepositoryTests : DatabaseTest
    {
        [Test]
        public async Task AddArticleAsync()
        {
            var dbContext = CreateDbContext();

            var repository = new ArticleRepository(dbContext);

            var article = new Article
            {
                Content = "content",
                Created = DateTime.Now,
                CreatorId = "id",
                Description = "description",
                Title = "title",
            };

            await repository.AddArticleAsync(article);

            Assert.That(dbContext.Article.Contains(article));
        }

        [Test]
        public async Task DeleteArticleByIdAsync()
        {
            var articles = new List<Article>
            {
                new Article { Id = 1, Content = "content", Title = "title", CreatorId = "id" },
                new Article { Id = 2, Content = "content", Title = "title", CreatorId = "id" },
                new Article { Id = 3, Content = "content", Title = "title", CreatorId = "id" },
                new Article { Id = 4, Content = "content", Title = "title", CreatorId = "id" },
            };

            var dbContext = CreateDbContext();

            await dbContext.AddContent(articles);

            var repository = new ArticleRepository(dbContext);

            const int Id = 3;

            await repository.DeleteArticleByIdAsync(Id);

            Assert.That(!dbContext.Article.Any(x => x.Id == Id));
        }

        [Test]
        public async Task GetArticleById()
        {
            var articles = new List<Article>
            {
                new Article { Id = 1, Content = "content1", Title = "title1", CreatorId = "id", Creator = new ApplicationUser { NickName = "nick" } },
                new Article { Id = 2, Content = "content2", Title = "title2", CreatorId = "id", Creator = new ApplicationUser { NickName = "nick" } },
                new Article { Id = 3, Content = "content3", Title = "title3", CreatorId = "id", Creator = new ApplicationUser { NickName = "nick" } },
                new Article { Id = 4, Content = "content4", Title = "title4", CreatorId = "id", Creator = new ApplicationUser { NickName = "nick" } },
            };

            var dbContext = CreateDbContext();

            await dbContext.AddContent(articles);

            var repository = new ArticleRepository(dbContext);

            const int Id = 3;

            var art = dbContext.Article.ToList();

            var res = repository.GetArticleById(Id, x => x);

            Assert.That(res.Title, Is.EqualTo(articles.FirstOrDefault(x => x.Id == Id).Title));
        }

        [Test]
        public async Task GetArticles()
        {
            var articles = new List<Article>
            {
                new Article { Id = 1, Content = "content1", Title = "title1", CreatorId = "id", Created = DateTime.Now, Creator = new ApplicationUser { NickName = "nick" } },
                new Article { Id = 2, Content = "content2", Title = "title2", CreatorId = "id", Created = DateTime.Now.AddDays(1), Creator = new ApplicationUser { NickName = "nick" } },
                new Article { Id = 3, Content = "content3", Title = "title3", CreatorId = "id", Created = DateTime.Now.AddDays(2), Creator = new ApplicationUser { NickName = "nick" } },
                new Article { Id = 4, Content = "content4", Title = "title4", CreatorId = "id", Created = DateTime.Now.AddDays(3), Creator = new ApplicationUser { NickName = "nick" } },
                new Article { Id = 5, Content = "content5", Title = "title5", CreatorId = "id", Created = DateTime.Now.AddDays(-2), Creator = new ApplicationUser { NickName = "nick" } },
                new Article { Id = 6, Content = "content6", Title = "title6", CreatorId = "id", Created = DateTime.Now.AddDays(-4), Creator = new ApplicationUser { NickName = "nick" } },
                new Article { Id = 7, Content = "content7", Title = "title7", CreatorId = "id", Created = DateTime.Now.AddDays(-5), Creator = new ApplicationUser { NickName = "nick" } },
            };

            var dbContext = CreateDbContext();

            await dbContext.AddContent(articles);

            var repository = new ArticleRepository(dbContext);

            const int Index = 2;
            const int Size = 3;

            var res = repository.GetArticles(Index, Size, x => x);

            Assert.That(res, Is.EquivalentTo(articles.OrderByDescending(x => x.Created).Skip(Size * Index).Take(Size)));
        }

        [Test]
        public async Task GetUserArticles()
        {
            var articles = new List<Article>
            {
                new Article { Id = 1, Content = "content1", Title = "title1", CreatorId = "id", Created = DateTime.Now, Creator = new ApplicationUser { NickName = "nick" } },
                new Article { Id = 2, Content = "content2", Title = "title2", CreatorId = "id2", Created = DateTime.Now.AddDays(1), Creator = new ApplicationUser { NickName = "nick" } },
                new Article { Id = 3, Content = "content3", Title = "title3", CreatorId = "id", Created = DateTime.Now.AddDays(2), Creator = new ApplicationUser { NickName = "nick" } },
                new Article { Id = 4, Content = "content4", Title = "title4", CreatorId = "id3", Created = DateTime.Now.AddDays(3), Creator = new ApplicationUser { NickName = "nick" } },
                new Article { Id = 5, Content = "content5", Title = "title5", CreatorId = "id", Created = DateTime.Now.AddDays(-2), Creator = new ApplicationUser { NickName = "nick" } },
                new Article { Id = 6, Content = "content6", Title = "title6", CreatorId = "id4", Created = DateTime.Now.AddDays(-4), Creator = new ApplicationUser { NickName = "nick" } },
                new Article { Id = 7, Content = "content7", Title = "title7", CreatorId = "id5", Created = DateTime.Now.AddDays(-5), Creator = new ApplicationUser { NickName = "nick" } },
            };

            var dbContext = CreateDbContext();

            await dbContext.AddContent(articles);

            var repository = new ArticleRepository(dbContext);

            const string UserId = "id";
            const int Index = 2;
            const int Size = 1;

            var res = repository.GetUserArticles(UserId, Index, Size, x => x);

            Assert.That(res, Is.EquivalentTo(articles.Where(x => x.CreatorId == UserId)
                .OrderByDescending(x => x.Created).Skip(Size * Index).Take(Size)));
        }
    }
}
