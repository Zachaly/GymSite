using GymSite.Application;
using GymSite.Application.Abstractions;
using GymSite.Database.Repository.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Models.Exercise;
using GymSite.Models.Exercise.Request;
using GymSite.Models.Response;
using Moq;

namespace GymSite.Tests.Unit.Service
{
    [TestFixture]
    public class ExerciseServiceTests
    {
        [Test]
        public async Task AddExercise()
        {
            var exerciseList = new List<Exercise>();

            var repositoryMock = new Mock<IExerciseRepository>();

            repositoryMock.Setup(x => x.AddExerciseAsync(It.IsAny<Exercise>()))
                .Callback((Exercise exercise) => exerciseList.Add(exercise));

            var responseFactoryMock = new Mock<IResponseFactory>();

            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<string>()))
                .Returns(new ResponseModel { Success = true });

            var exerciseFactoryMock = new Mock<IExerciseFactory>();

            exerciseFactoryMock.Setup(x => x.Create(It.IsAny<AddExerciseRequest>()))
                .Returns((AddExerciseRequest request) => new Exercise
                {
                    Name = request.Name,
                });

            var service = new ExerciseService(repositoryMock.Object, responseFactoryMock.Object, exerciseFactoryMock.Object);

            var request = new AddExerciseRequest
            {
                Description = "desc",
                Name = "name",
                UserId = "userId",
            };

            var res = await service.AddExercise(request);

            Assert.Multiple(() =>
            {
                Assert.That(exerciseList.Any(x => x.Name == request.Name));
                Assert.That(res.Success);
            });
        }

        [Test]
        public async Task GetExerciseById()
        {
            var exerciseList = new List<Exercise>
            {
                new Exercise { Id = 1 },
                new Exercise { Id = 2 },
                new Exercise { Id = 3 },
            };

            var repositoryMock = new Mock<IExerciseRepository>();

            repositoryMock.Setup(x => x.GetExerciseById(It.IsAny<int>(), It.IsAny<Func<Exercise, ExerciseModel>>()))
                .Returns((int id, Func<Exercise, ExerciseModel> selector) 
                    => exerciseList.Where(x => x.Id == id).Select(selector).FirstOrDefault());

            var responseFactoryMock = new Mock<IResponseFactory>();

            responseFactoryMock.Setup(x => x.CreateSuccess( It.IsAny<ExerciseModel>(),It.IsAny<string>()))
                .Returns((ExerciseModel data, string _) => 
                    new DataResponseModel<ExerciseModel> { Success = true, Data = data });

            var exerciseFactoryMock = new Mock<IExerciseFactory>();

            exerciseFactoryMock.Setup(x => x.CreateModel(It.IsAny<Exercise>()))
                .Returns((Exercise exercise) => new ExerciseModel
                {
                    Id = exercise.Id,
                });

            var service = new ExerciseService(repositoryMock.Object, responseFactoryMock.Object, exerciseFactoryMock.Object);
            const int Id = 2;

            var res = service.GetExerciseById(Id);

            Assert.Multiple(() =>
            {
                Assert.That(res.Data.Id, Is.EqualTo(Id));
                Assert.That(res.Success);
            });
        }

        [Test]
        public async Task GetUserExercises()
        {
            var exerciseList = new List<Exercise>
            {
                new Exercise { Id = 1, UserId = "id" },
                new Exercise { Id = 2, UserId = "id2" },
                new Exercise { Id = 3, UserId = "id" },
            };

            var repositoryMock = new Mock<IExerciseRepository>();

            repositoryMock.Setup(x => x.GetExercisesByUserId(It.IsAny<string>(), It.IsAny<Func<Exercise, ExerciseListItemModel>>()))
                .Returns((string id, Func<Exercise, ExerciseListItemModel> selector) 
                    => exerciseList.Where(x => x.UserId == id).Select(selector).ToList());

            var responseFactoryMock = new Mock<IResponseFactory>();

            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<IEnumerable<ExerciseListItemModel>>(), ""))
                    .Returns((IEnumerable<ExerciseListItemModel> data, string _)
                        => new DataResponseModel<IEnumerable<ExerciseListItemModel>> { Success = true, Data = data });

            var exerciseFactoryMock = new Mock<IExerciseFactory>();

            exerciseFactoryMock.Setup(x => x.CreateListItem(It.IsAny<Exercise>()))
                .Returns((Exercise exercise) => new ExerciseListItemModel
                {
                    Id = exercise.Id,
                });

            var service = new ExerciseService(repositoryMock.Object, responseFactoryMock.Object, exerciseFactoryMock.Object);
            const string UserId = "id";

            var res = service.GetUserExercises(UserId);

            Assert.Multiple(() =>
            {
                Assert.That(res.Data.Count(), Is.EqualTo(exerciseList.Where(x => x.UserId == UserId).Count()));
                Assert.That(res.Success);
            });
        }

        [Test]
        public async Task RemoveExerciseById_Success()
        {
            var exerciseList = new List<Exercise>
            {
                new Exercise { Id = 1 },
                new Exercise { Id = 2 },
                new Exercise { Id = 3 },
            };

            var repositoryMock = new Mock<IExerciseRepository>();

            repositoryMock.Setup(x => x.RemoveExerciseByIdAsync(It.IsAny<int>()))
                .Callback((int id)
                    => exerciseList.Remove(exerciseList.FirstOrDefault(x => x.Id == id)));

            var responseFactoryMock = new Mock<IResponseFactory>();

            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<string>()))
                .Returns(new ResponseModel { Success = true });

            var exerciseFactoryMock = new Mock<IExerciseFactory>();

            var service = new ExerciseService(repositoryMock.Object, responseFactoryMock.Object, exerciseFactoryMock.Object);
            const int Id = 2;

            var res = await service.RemoveExercise(Id);

            Assert.Multiple(() =>
            {
                Assert.That(!exerciseList.Any(x => x.Id == Id));
                Assert.That(res.Success);
            });
        }

        [Test]
        public async Task RemoveExerciseById_Fail()
        {
            var exerciseList = new List<Exercise>
            {
                new Exercise { Id = 1 },
                new Exercise { Id = 2 },
                new Exercise { Id = 3 },
            };

            var repositoryMock = new Mock<IExerciseRepository>();

            repositoryMock.Setup(x => x.RemoveExerciseByIdAsync(It.IsAny<int>()))
                .Callback((int id)
                    => throw new Exception());

            var responseFactoryMock = new Mock<IResponseFactory>();

            responseFactoryMock.Setup(x => x.CreateFail(It.IsAny<string>(), null))
                .Returns(new ResponseModel { Success = false });

            var exerciseFactoryMock = new Mock<IExerciseFactory>();

            var service = new ExerciseService(repositoryMock.Object, responseFactoryMock.Object, exerciseFactoryMock.Object);
            const int Id = 2;

            var res = await service.RemoveExercise(Id);

            Assert.Multiple(() =>
            {
                Assert.That(exerciseList.Any(x => x.Id == Id));
                Assert.That(!res.Success);
            });
        }
    }
}
