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
    public class WorkoutServiceTests
    {
        [Test]
        public void GetUserWorkouts()
        {
            var workouts = new List<Workout>
            {
                new Workout { Id = 1, UserId = "id" },
                new Workout { Id = 2, UserId = "id2" },
                new Workout { Id = 3, UserId = "id" },
                new Workout { Id = 4, UserId = "id2" },
                new Workout { Id = 5, UserId = "id" },
            };

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<IEnumerable<WorkoutListItemModel>>(), It.IsAny<string>()))
                .Returns((IEnumerable<WorkoutListItemModel> data, string _) => new DataResponseModel<IEnumerable<WorkoutListItemModel>>
                {
                    Data = data,
                    Success = true
                });

            var repositoryMock = new Mock<IWorkoutRepository>();
            repositoryMock.Setup(x => x.GetUserWorkouts(It.IsAny<string>(), It.IsAny<Func<Workout, WorkoutListItemModel>>()))
                .Returns((string id, Func<Workout, WorkoutListItemModel> selector)
                    => workouts.Where(x => x.UserId == id).Select(selector));

            var workoutFactoryMock = new Mock<IWorkoutFactory>();
            workoutFactoryMock.Setup(x => x.CreateListItem(It.IsAny<Workout>()))
                .Returns((Workout workout) => new WorkoutListItemModel { Id = workout.Id });

            var service = new WorkoutService(repositoryMock.Object, workoutFactoryMock.Object, responseFactoryMock.Object);

            const string UserId = "id";
            var res = service.GetUserWorkouts(UserId);

            Assert.That(res.Data.Count(), Is.EqualTo(workouts.Where(x => x.UserId == UserId).Count()));
        }

        [Test]
        public void GetWorkoutById()
        {
            var workouts = new List<Workout>
            {
                new Workout { Id = 1, UserId = "id", Name = "wrk1" },
                new Workout { Id = 2, UserId = "id2", Name = "wrk2" },
                new Workout { Id = 3, UserId = "id", Name = "wrk3" },
                new Workout { Id = 4, UserId = "id2", Name = "wrk4" },
                new Workout { Id = 5, UserId = "id", Name = "wrk5" },
            };

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<WorkoutModel>(), It.IsAny<string>()))
                .Returns((WorkoutModel data, string _) => new DataResponseModel<WorkoutModel>
                {
                    Data = data,
                    Success = true
                });

            var repositoryMock = new Mock<IWorkoutRepository>();
            repositoryMock.Setup(x => x.GetWorkoutById(It.IsAny<int>(), It.IsAny<Func<Workout, WorkoutModel>>()))
                .Returns((int id, Func<Workout, WorkoutModel> selector)
                    => workouts.Where(x => x.Id == id).Select(selector).FirstOrDefault());

            var workoutFactoryMock = new Mock<IWorkoutFactory>();
            workoutFactoryMock.Setup(x => x.CreateModel(It.IsAny<Workout>()))
                .Returns((Workout workout) => new WorkoutModel { Id = workout.Id, Name = workout.Name });

            var service = new WorkoutService(repositoryMock.Object, workoutFactoryMock.Object, responseFactoryMock.Object);

            const int Id = 3;
            var res = service.GetWorkoutById(Id);

            Assert.That(res.Data.Name, Is.EqualTo(workouts.FirstOrDefault(x => x.Id == Id).Name));
        }

        [Test]
        public async Task AddWorkoutAsync()
        {
            var workouts = new List<Workout>
            {
                new Workout { Id = 1, UserId = "id", Name = "wrk1" },
                new Workout { Id = 2, UserId = "id2", Name = "wrk2" },
                new Workout { Id = 3, UserId = "id", Name = "wrk3" },
                new Workout { Id = 4, UserId = "id2", Name = "wrk4" },
                new Workout { Id = 5, UserId = "id", Name = "wrk5" },
            };

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<string>()))
                .Returns(new ResponseModel { Success = true });

            var repositoryMock = new Mock<IWorkoutRepository>();
            repositoryMock.Setup(x => x.AddWorkoutAsync(It.IsAny<Workout>()))
                .Callback((Workout workout) => workouts.Add(workout));

            var workoutFactoryMock = new Mock<IWorkoutFactory>();
            workoutFactoryMock.Setup(x => x.Create(It.IsAny<AddWorkoutRequest>()))
                .Returns((AddWorkoutRequest request) => new Workout { Name = request.Name });

            var service = new WorkoutService(repositoryMock.Object, workoutFactoryMock.Object, responseFactoryMock.Object);

            var request = new AddWorkoutRequest
            {
                Description = "desc",
                Name = "name",
                UserId = "id"
            };

            const int Id = 3;
            var res = await service.AddWorkoutAsync(request);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(workouts.Any(x => x.Name == request.Name));
            });
        }

        [Test]
        public async Task UpdateWorkoutAsync()
        {
            var workouts = new List<Workout>
            {
                new Workout { Id = 1, UserId = "id", Name = "wrk1" },
                new Workout { Id = 2, UserId = "id2", Name = "wrk2" },
                new Workout { Id = 3, UserId = "id", Name = "wrk3" },
                new Workout { Id = 4, UserId = "id2", Name = "wrk4" },
                new Workout { Id = 5, UserId = "id", Name = "wrk5" },
            };

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<string>()))
                .Returns(new ResponseModel { Success = true });

            var repositoryMock = new Mock<IWorkoutRepository>();
            repositoryMock.Setup(x => x.UpdateWorkoutAsync(It.IsAny<Workout>()));
            repositoryMock.Setup(x => x.GetWorkoutById(It.IsAny<int>(), It.IsAny<Func<Workout, Workout>>()))
                .Returns((int id, Func<Workout, Workout> _) => workouts.FirstOrDefault(x => x.Id == id));

            var workoutFactoryMock = new Mock<IWorkoutFactory>();
            workoutFactoryMock.Setup(x => x.Create(It.IsAny<AddWorkoutRequest>()))
                .Returns((AddWorkoutRequest request) => new Workout { Name = request.Name });

            var service = new WorkoutService(repositoryMock.Object, workoutFactoryMock.Object, responseFactoryMock.Object);

            var request = new UpdateWorkoutRequest
            {
                Description = "updated desc",
                Name = "updated name",
                Id = 3
            };

            var res = await service.UpdateWorkoutAsync(request);

            var workout = workouts.FirstOrDefault(workouts => workouts.Id == request.Id);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(workout.Name, Is.EqualTo(request.Name));
                Assert.That(workout.Description, Is.EqualTo(request.Description));
            });
        }

        [Test]
        public async Task DeleteWorkoutByIdAsync()
        {
            var workouts = new List<Workout>
            {
                new Workout { Id = 1, UserId = "id", Name = "wrk1" },
                new Workout { Id = 2, UserId = "id2", Name = "wrk2" },
                new Workout { Id = 3, UserId = "id", Name = "wrk3" },
                new Workout { Id = 4, UserId = "id2", Name = "wrk4" },
                new Workout { Id = 5, UserId = "id", Name = "wrk5" },
            };

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<string>()))
                .Returns(new ResponseModel { Success = true });

            var repositoryMock = new Mock<IWorkoutRepository>();
            repositoryMock.Setup(x => x.DeleteWorkoutByIdAsync(It.IsAny<int>()))
                .Callback((int id) => workouts.Remove(workouts.FirstOrDefault(x => x.Id == id)));

            var workoutFactoryMock = new Mock<IWorkoutFactory>();

            var service = new WorkoutService(repositoryMock.Object, workoutFactoryMock.Object, responseFactoryMock.Object);

            const int Id = 3;
            var res = await service.DeleteWorkoutByIdAsync(Id);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(!workouts.Any(x => x.Id == Id));
            });
        }
    }
}
