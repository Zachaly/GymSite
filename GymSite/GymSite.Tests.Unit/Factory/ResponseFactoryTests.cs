using GymSite.Application.Response;
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

            const ResponseCode Code = ResponseCode.Ok;
            const string Message = "message";

            var response = factory.CreateSuccess(Code, Message);

            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Success, Is.True);
                Assert.That(response.Code, Is.EqualTo(Code));
                Assert.That(response.Message, Is.EqualTo(Message));
                Assert.That(response.Errors, Is.Null);
            });
        }

        [Test]
        public void CreateFailure_ResponseModel()
        {
            var factory = new ResponseFactory();

            const ResponseCode Code = ResponseCode.BadRequest;
            const string Message = "message";
            var errors = new Dictionary<string, IEnumerable<string>>();

            var response = factory.CreateFail(Code, Message, errors);

            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Success, Is.False);
                Assert.That(response.Code, Is.EqualTo(Code));
                Assert.That(response.Message, Is.EqualTo(Message));
                Assert.That(response.Errors, Is.EquivalentTo(errors));
            });
        }

        [Test]
        public void CreateSuccess_DataResponseModel()
        {
            var factory = new ResponseFactory();

            const ResponseCode Code = ResponseCode.Ok;
            const string Message = "message";
            const int Data = 2137;

            var response = factory.CreateSuccess(Code, Message, Data);

            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Success, Is.True);
                Assert.That(response.Code, Is.EqualTo(Code));
                Assert.That(response.Message, Is.EqualTo(Message));
                Assert.That(response.Errors, Is.Null);
                Assert.That(response.Data, Is.EqualTo(Data));
            });
        }

        [Test]
        public void CreateFailure_DataResponseModel()
        {
            var factory = new ResponseFactory();

            const ResponseCode Code = ResponseCode.BadRequest;
            const string Message = "message";
            var errors = new Dictionary<string, IEnumerable<string>>();

            var response = factory.CreateFail<int?>(Code, Message, errors);

            Assert.Multiple(() =>
            {
                Assert.That(response, Is.Not.Null);
                Assert.That(response.Success, Is.False);
                Assert.That(response.Code, Is.EqualTo(Code));
                Assert.That(response.Message, Is.EqualTo(Message));
                Assert.That(response.Errors, Is.EquivalentTo(errors));
                Assert.That(response.Data, Is.Null);
            });
        }
    }
}
