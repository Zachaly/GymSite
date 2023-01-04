using GymSite.Models.Article.Request;
using GymSite.Models.Article.Validator;

namespace GymSite.Tests.Unit.Validator
{
    public class AddArticleValidatorTests
    {
        [Test]
        public void ValidData_Pass()
        {
            var request = new AddArticleRequest
            {
                Content = new string('a', 100),
                CreatorId = "id",
                Description = "description",
                Title = "title",
            };

            var res = new AddArticleValidator().Validate(request);

            Assert.That(res.IsValid);
        }

        [Test]
        public void OnlyRequiredData_Pass()
        {
            var request = new AddArticleRequest
            {
                Content = new string('a', 100),
                CreatorId = "id",
                Title = "title",
            };

            var res = new AddArticleValidator().Validate(request);

            Assert.That(res.IsValid);
        }

        [Test]
        public void EmptyData_Fail()
        {
            var request = new AddArticleRequest
            {
            };

            var res = new AddArticleValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void ContentBelowMinLength_Fail()
        {
            var request = new AddArticleRequest
            {
                Content = new string('a', 99),
                CreatorId = "id",
                Description = "description",
                Title = "title",
            };

            var res = new AddArticleValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void ContentExceedsMaxLength_Fail()
        {
            var request = new AddArticleRequest
            {
                Content = new string('a', 1501),
                CreatorId = "id",
                Description = "description",
                Title = "title",
            };

            var res = new AddArticleValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void TitleBelowMinLength_Fail()
        {
            var request = new AddArticleRequest
            {
                Content = new string('a', 100),
                CreatorId = "id",
                Description = "description",
                Title = new string('a', 4),
            };

            var res = new AddArticleValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void TitleExceedsMaxLength_Fail()
        {
            var request = new AddArticleRequest
            {
                Content = new string('a', 100),
                CreatorId = "id",
                Description = "description",
                Title = new string('a', 51),
            };

            var res = new AddArticleValidator().Validate(request);

            Assert.That(!res.IsValid);
        }

        [Test]
        public void DescriptionExceedsMaxLength_Fail()
        {
            var request = new AddArticleRequest
            {
                Content = new string('a', 100),
                CreatorId = "id",
                Description = new string('a', 101),
                Title = "title",
            };

            var res = new AddArticleValidator().Validate(request);

            Assert.That(!res.IsValid);
        }
    }
}
