using GymSite.Database.User;
using GymSite.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
