using GymSite.Application;
using GymSite.Application.Abstractions;
using GymSite.Application.Response.Abstractions;
using GymSite.Database.Repository.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Models.Record.Request;
using GymSite.Models.Response;
using Moq;

namespace GymSite.Tests.Unit.Service
{
    [TestFixture]
    public class ExerciseRecordServiceTests
    {
        [Test]
        public async Task AddRecord()
        {
            var recordList = new List<ExerciseRecord>();

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<ResponseCode>(), It.IsAny<string>()))
                .Returns(new ResponseModel { Success = true });

            var repositoryMock = new Mock<IExerciseRecordRepository>();
            repositoryMock.Setup(x => x.AddRecordAsync(It.IsAny<ExerciseRecord>()))
                .Callback((ExerciseRecord record) => recordList.Add(record));

            var factoryMock = new Mock<IExerciseRecordFactory>();
            factoryMock.Setup(x => x.Create(It.IsAny<AddExerciseRecordRequest>()))
                .Returns((AddExerciseRecordRequest request) => new ExerciseRecord { Reps = request.Reps });

            var service = new ExerciseRecordService(responseFactoryMock.Object, repositoryMock.Object, factoryMock.Object);

            var request = new AddExerciseRecordRequest { Reps = 1 };

            var res = await service.AddRecord(request);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(recordList.Any(x => x.Reps == request.Reps));
                Assert.That(recordList.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public async Task RemoveRecord_Success()
        {
            var recordList = new List<ExerciseRecord>
            {
                new ExerciseRecord { Id = 1 },
                new ExerciseRecord { Id = 2 },
                new ExerciseRecord { Id = 3 },
            };

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<ResponseCode>(), It.IsAny<string>()))
                .Returns(new ResponseModel { Success = true });

            var repositoryMock = new Mock<IExerciseRecordRepository>();
            repositoryMock.Setup(x => x.RemoveRecordByIdAsync(It.IsAny<int>()))
                .Callback((int id) => recordList.Remove(recordList.FirstOrDefault(x => x.Id == id)));

            var factoryMock = new Mock<IExerciseRecordFactory>();

            var service = new ExerciseRecordService(responseFactoryMock.Object, repositoryMock.Object, factoryMock.Object);

            const int Id = 2;

            var res = await service.RemoveRecord(Id);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(!recordList.Any(x => x.Id == Id));
            });
        }

        [Test]
        public async Task RemoveRecord_Fail()
        {
            var recordList = new List<ExerciseRecord>
            {
                new ExerciseRecord { Id = 1 },
                new ExerciseRecord { Id = 2 },
                new ExerciseRecord { Id = 3 },
            };

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateFail(It.IsAny<ResponseCode>(), It.IsAny<string>(), null))
                .Returns(new ResponseModel { Success = false });

            var repositoryMock = new Mock<IExerciseRecordRepository>();
            repositoryMock.Setup(x => x.RemoveRecordByIdAsync(It.IsAny<int>()))
                .Callback((int id) => throw new Exception());

            var factoryMock = new Mock<IExerciseRecordFactory>();

            var service = new ExerciseRecordService(responseFactoryMock.Object, repositoryMock.Object, factoryMock.Object);

            const int Id = 2;

            var res = await service.RemoveRecord(Id);

            Assert.Multiple(() =>
            {
                Assert.That(!res.Success);
                Assert.That(recordList.Any(x => x.Id == Id));
            });
        }
    }
}
