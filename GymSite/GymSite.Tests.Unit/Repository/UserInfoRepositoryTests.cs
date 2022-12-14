using GymSite.Database.Repository;
using GymSite.Domain.Entity;

namespace GymSite.Tests.Unit.Repository
{
    [TestFixture]
    public class UserInfoRepositoryTests : DatabaseTest
    {
        [Test]
        public async Task AddInfo()
        {
            var dbContext = CreateDbContext();

            var repository = new UserInfoRepository(dbContext);

            var info = new UserInfo { Id = 1, UserId = "id" };

            await repository.AddInfoAsync(info);

            Assert.That(dbContext.UserInfo.Contains(info));
        }

        [Test]
        public async Task GetInfo()
        {
            var infoList = new List<UserInfo>
            {
                new UserInfo{ Id = 1, UserId = "id1" },
                new UserInfo{ Id = 2, UserId = "id2" },
                new UserInfo{ Id = 3, UserId = "id3" },
            };
            var dbContext = CreateDbContext();
            await dbContext.AddContent(infoList);

            var repository = new UserInfoRepository(dbContext);

            const int Id = 2;

            var res = await repository.GetInfoAsync(Id, x => x);

            Assert.That(res.Id, Is.EqualTo(Id));
        }
    }
}
