using GymSite.Application;
using GymSite.Models.Response;

namespace GymSite.Tests.Unit.Factory
{
    [TestFixture]
    public class ResponseFactoryTests
    {
        [Test]
        public void CreateSuccess_ResponseModel()
        {
            var factory = new ResponseFactory();

            const string Message = "message";

            var response = factory.CreateSuccess(Message);

            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Success, Is.True);
                Assert.That(response.Message, Is.EqualTo(Message));
                Assert.That(response.ValidationErrors, Is.Null);
            });
        }

        [Test]
        public void CreateFailure_ResponseModel()
        {
            var factory = new ResponseFactory();

            const string Message = "message";
            var errors = new Dictionary<string, IEnumerable<string>>();

            var response = factory.CreateFail(Message, errors);

            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Success, Is.False);
                Assert.That(response.Message, Is.EqualTo(Message));
                Assert.That(response.ValidationErrors, Is.EquivalentTo(errors));
            });
        }

        [Test]
        public void CreateSuccess_DataResponseModel()
        {
            var factory = new ResponseFactory();

            const string Message = "message";
            const int Data = 2137;

            var response = factory.CreateSuccess(Data, Message);

            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Success, Is.True);
                Assert.That(response.Message, Is.EqualTo(Message));
                Assert.That(response.ValidationErrors, Is.Null);
                Assert.That(response.Data, Is.EqualTo(Data));
            });
        }

        [Test]
        public void CreateFailure_DataResponseModel()
        {
            var factory = new ResponseFactory();

            const string Message = "message";
            var errors = new Dictionary<string, IEnumerable<string>>();

            var response = factory.CreateFail<int?>(Message, errors);

            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Success, Is.False);
                Assert.That(response.Message, Is.EqualTo(Message));
                Assert.That(response.ValidationErrors, Is.EquivalentTo(errors));
                Assert.That(response.Data, Is.Null);
            });
        }
    }
}
