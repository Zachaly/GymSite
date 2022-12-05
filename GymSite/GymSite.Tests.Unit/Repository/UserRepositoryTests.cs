using GymSite.Database.User;
using GymSite.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace GymSite.Tests.Unit.Repository
{
    [TestFixture]
    public class UserRepositoryTests : DatabaseTest
    {
        [Test]
        public async Task GetUserByUsername()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser {Id = "id1", UserName = "name1", NickName = "nick" },
                new ApplicationUser {Id = "id2", UserName = "name2", NickName = "nick" },
                new ApplicationUser {Id = "id3", UserName = "name3", NickName = "nick" },
            };

            var dbContext = CreateDbContext();
            await dbContext.AddContent(userList);

            var repository = new UserRepository(dbContext);

            const string UserName = "name2";

            var user = await repository.GetUserByNameAsync(UserName, x => x);

            Assert.Multiple(() =>
            {
                Assert.That(user.Id, Is.EqualTo(userList.Where(x => x.UserName == UserName).First().Id));
                Assert.That(user.UserName, Is.EqualTo(UserName));
            });
        }

        [Test]
        public async Task GetUserById()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser {Id = "id1", UserName = "name1", NickName = "nick" },
                new ApplicationUser {Id = "id2", UserName = "name2", NickName = "nick", UserInfoId = 1 },
                new ApplicationUser {Id = "id3", UserName = "name3", NickName = "nick" },
            };

            var userInfo = new List<UserInfo>
            {
                new UserInfo { UserId = "id2", Id = 1 }
            };

            var dbContext = CreateDbContext();
            await dbContext.AddContent(userList);
            await dbContext.AddContent(userInfo);

            var repository = new UserRepository(dbContext);

            const string Id = "id2";

            var user = await repository.GetUserByIdAsync(Id, x => x);

            Assert.Multiple(() =>
            {
                Assert.That(user.Id, Is.EqualTo(userList.Where(x => x.Id == Id).First().Id));
                Assert.That(user.UserInfo, Is.Not.Null);
            });
        }

        [Test]
        public async Task UpdateUser()
        {
            var userList = new List<ApplicationUser>
            {
                new ApplicationUser {Id = "id1", UserName = "name1", NickName = "nick" },
                new ApplicationUser {Id = "id2", UserName = "name2", NickName = "nick", UserInfoId = 1 },
                new ApplicationUser {Id = "id3", UserName = "name3", NickName = "nick" },
            };

            var userInfo = new List<UserInfo>
            {
                new UserInfo { UserId = "id2", Id = 1 }
            };

            var dbContext = CreateDbContext();
            await dbContext.AddContent(userList);
            await dbContext.AddContent(userInfo);

            var repository = new UserRepository(dbContext);

            const string Id = "id2";
            const string NewNick = "new nickname";
            const string NewFName = "new fname";

            var user = dbContext.Users.Include(user => user.UserInfo).FirstOrDefault(x => x.Id == Id);

            user.NickName = NewNick;
            user.UserInfo.FirstName = NewFName;

            await repository.UpdateUser(user);

            user = dbContext.Users.Include(user => user.UserInfo).FirstOrDefault(x => x.Id == Id);

            Assert.Multiple(() =>
            {
                Assert.That(user.NickName, Is.EqualTo(NewNick));
                Assert.That(user.UserInfo.FirstName, Is.EqualTo(NewFName));
            });
        }
    }
}
