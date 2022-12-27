using GymSite.Application.Abstractions;
using GymSite.Application;
using GymSite.Database.Repository.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Models.Response;
using GymSite.Models.Workout.Requests;
using Moq;
using GymSite.Models.Workout;

namespace GymSite.Tests.Unit.Service
{
    [TestFixture]
    public class ExerciseSetServiceTests
    {
        [Test]
        public async Task AddExerciseSetAsync()
        {
            var sets = new List<ExerciseSet>();

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<ExerciseSetModel>(), It.IsAny<string>()))
                .Returns((ExerciseSetModel data, string _) => new DataResponseModel<ExerciseSetModel>
                {
                    Success = true,
                    Data = data,
                });

            var repositoryMock = new Mock<IExerciseSetRepository>();
            repositoryMock.Setup(x => x.AddExerciseSetAsync(It.IsAny<ExerciseSet>()))
                .Callback((ExerciseSet workoutExercise) =>
                {
                    sets.Add(workoutExercise);
                });

            var exerciseSetFactoryMock = new Mock<IExerciseSetFactory>();
            exerciseSetFactoryMock.Setup(x => x.Create(It.IsAny<AddExerciseSetRequest>()))
                .Returns((AddExerciseSetRequest request) => new ExerciseSet
                {
                    ExerciseId = request.WorkoutExerciseId,
                    Reps = request.Reps,
                    Weigth = request.Weight
                });

            exerciseSetFactoryMock.Setup(x => x.CreateModel(It.IsAny<ExerciseSet>()))
                .Returns((ExerciseSet set) => new ExerciseSetModel { Id = set.Id });

            var service = new ExerciseSetService(repositoryMock.Object, exerciseSetFactoryMock.Object, responseFactoryMock.Object);

            var request = new AddExerciseSetRequest
            {
                WorkoutExerciseId = 1,
                Reps = 2,
                Weight = 3
            };

            var res = await service.AddExerciseSetAsync(request);

            Assert.Multiple(() =>
            {
                Assert.That(res.Data, Is.Not.Null);
                Assert.That(res.Success);
                Assert.That(sets.Any(x 
                    => x.ExerciseId == request.WorkoutExerciseId && x.Reps == request.Reps && x.Weigth == request.Weight));
            });
        }

        [Test]
        public async Task DeleteExerciseSetAsync()
        {
            var sets = new List<ExerciseSet>
            {
                new ExerciseSet { Id = 1 },
                new ExerciseSet { Id = 2 },
                new ExerciseSet { Id = 3 },
                new ExerciseSet { Id = 4 },
            };

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<string>()))
                .Returns(new ResponseModel { Success = true });

            var repositoryMock = new Mock<IExerciseSetRepository>();
            repositoryMock.Setup(x => x.RemoveExerciseSetByIdAsync(It.IsAny<int>()))
                .Callback((int id) => sets.Remove(sets.FirstOrDefault(x => x.Id == id)));

            var exerciseSetFactoryMock = new Mock<IExerciseSetFactory>();

            var service = new ExerciseSetService(repositoryMock.Object, exerciseSetFactoryMock.Object, responseFactoryMock.Object);

            const int Id = 3;

            var res = await service.DeleteExerciseSetByIdAsync(Id);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(!sets.Any(x => x.Id == Id));
            });
        }
    }
}
