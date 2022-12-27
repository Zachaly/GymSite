using GymSite.Application;
using GymSite.Application.Abstractions;
using GymSite.Database.Repository.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Models.Response;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSite.Tests.Unit.Service
{
    [TestFixture]
    public class WorkoutExerciseServiceTests
    {
        [Test]
        public async Task AddWorkoutExercise()
        {
            var exercises = new List<WorkoutExercise>();

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<WorkoutExerciseModel>(), It.IsAny<string>()))
                .Returns((WorkoutExerciseModel data, string _) => new DataResponseModel<WorkoutExerciseModel> 
                {
                    Success = true,
                    Data = data,
                });

            var repositoryMock = new Mock<IWorkoutExerciseRepository>();
            repositoryMock.Setup(x => x.AddWorkoutExerciseAsync(It.IsAny<WorkoutExercise>()))
                .Callback((WorkoutExercise workoutExercise) =>
                {
                    workoutExercise.Id = 1;
                    exercises.Add(workoutExercise);
                });

            repositoryMock.Setup(x => x.GetWorkoutExerciseById(It.IsAny<int>(), It.IsAny<Func<WorkoutExercise, WorkoutExercise>>()))
                .Returns((int id, Func<WorkoutExercise, WorkoutExercise> selector)
                    => exercises.Where(x => x.Id == id).Select(selector).FirstOrDefault());

            var workoutExerciseFactoryMock = new Mock<IWorkoutExerciseFactory>();
            workoutExerciseFactoryMock.Setup(x => x.Create(It.IsAny<AddWorkoutExerciseRequest>()))
                .Returns((AddWorkoutExerciseRequest request) => new WorkoutExercise
                {
                    ExerciseId = request.ExerciseId,
                    WorkoutId = request.WorkoutId
                });

            workoutExerciseFactoryMock.Setup(x => x.CreateModel(It.IsAny<WorkoutExercise>()))
                .Returns((WorkoutExercise exercise) => new WorkoutExerciseModel
                {
                    ExerciseId = exercise.ExerciseId,
                });

            var service = new WorkoutExerciseService(repositoryMock.Object, workoutExerciseFactoryMock.Object, responseFactoryMock.Object);

            var request = new AddWorkoutExerciseRequest
            {
                WorkoutId = 1,
                ExerciseId = 2,
            };

            var res = await service.AddWorkoutExerciseAsync(request);

            Assert.Multiple(() =>
            {
                Assert.That(res.Data, Is.Not.Null);
                Assert.That(res.Success);
                Assert.That(exercises.Any(x => x.WorkoutId == request.WorkoutId && x.ExerciseId == request.ExerciseId));
            });
        }

        [Test]
        public async Task DeleteWorkoutExerciseById()
        {
            var exercises = new List<WorkoutExercise>
            {
                new WorkoutExercise { Id = 1 },
                new WorkoutExercise { Id = 2 },
                new WorkoutExercise { Id = 3 },
                new WorkoutExercise { Id = 4 },
            };

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess( It.IsAny<string>()))
                .Returns(new ResponseModel { Success = true });

            var repositoryMock = new Mock<IWorkoutExerciseRepository>();

            repositoryMock.Setup(x => x.DeleteWorkoutExerciseAsync(It.IsAny<int>()))
                .Callback((int id) => exercises.Remove(exercises.FirstOrDefault(x => x.Id == id)));

            var workoutExerciseFactoryMock = new Mock<IWorkoutExerciseFactory>();

            var service = new WorkoutExerciseService(repositoryMock.Object, workoutExerciseFactoryMock.Object, responseFactoryMock.Object);

            const int Id = 3;

            var res = await service.DeleteWorkoutExerciseByIdAsync(Id);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(!exercises.Any(x => x.Id == Id));
            });
        }
    }
}
