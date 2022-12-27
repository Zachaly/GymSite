using GymSite.Domain.Entity;
using GymSite.Models.Response;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GymSite.Tests.Integration
{
    public class WorkoutControllerTests : IntegrationTest
    {
        [Fact]
        public async Task Get_Success()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;
            var workouts = new List<Workout>
            {
                new Workout { UserId = userId, Description = "desc1", Name = "name1" },
                new Workout { UserId = userId, Description = "desc2", Name = "name2" },
                new Workout { UserId = "id2", Description = "desc3", Name = "name3" },
                new Workout { UserId = "id", Description = "desc4", Name = "name4" },
                new Workout { UserId = userId, Description = "desc5", Name = "name5" },
            };

            await AddToDatabase(workouts);

            var response = await _httpClient.GetAsync($"api/workout?userId={userId}");

            var content = await response.Content.ReadFromJsonAsync <DataResponseModel<IEnumerable<WorkoutListItemModel>>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(content.Success);
            Assert.Equal(content.Data.Count(), workouts.Where(x => x.UserId == userId).Count());
        }

        [Fact]
        public async Task GetById_Success()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            const int Id = 3;

            var workouts = new List<Workout>
            {
                new Workout { Id = 1, UserId = userId, Description = "desc1", Name = "name1" },
                new Workout { Id = 2, UserId = userId, Description = "desc2", Name = "name2" },
                new Workout { Id = 3, UserId = userId, Description = "desc3", Name = "name3" },
                new Workout { Id = 4, UserId = userId, Description = "desc4", Name = "name4" },
                new Workout { Id = 5, UserId = userId, Description = "desc5", Name = "name5" },
            };

            var exercises = new List<Exercise>
            {
                new Exercise { Id = 1, Name = "exercise", Description = "desc", UserId = userId },
            };

            var workoutExercises = new List<WorkoutExercise>
            {
                new WorkoutExercise { Id = 1, ExerciseId = 1, WorkoutId = Id }
            };

            var exerciseSets = new List<ExerciseSet>
            {
                new ExerciseSet { Reps = 1, Weigth = 2, ExerciseId = 1 }
            };

            await AddToDatabase(workouts);
            await AddToDatabase(exercises);
            await AddToDatabase(workoutExercises);
            await AddToDatabase(exerciseSets);

            var response = await _httpClient.GetAsync($"api/workout/{Id}");

            var test = GetFromDatabase<Workout>().ToList();

            var content = await response.Content.ReadFromJsonAsync<DataResponseModel<WorkoutModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.True(content.Success);
            Assert.Equal(content.Data.Name, workouts.FirstOrDefault(x => x.Id == Id).Name);
            Assert.Equal(content.Data.Exercices.Count(), workoutExercises.Count);
            Assert.Equal(content.Data.Exercices.First().Sets.Count(), exerciseSets.Count);
        }

        [Fact]
        public async Task GetById_NotFound()
        {
            await Authenticate();

            var workouts = new List<Workout>
            {
                new Workout { Id = 1, UserId = "id", Description = "desc1", Name = "name1" },
                new Workout { Id = 2, UserId = "id", Description = "desc2", Name = "name2" },
                new Workout { Id = 3, UserId = "id2", Description = "desc3", Name = "name3" },
                new Workout { Id = 4, UserId = "id", Description = "desc4", Name = "name4" },
                new Workout { Id = 5, UserId = "id", Description = "desc5", Name = "name5" },
            };

            await AddToDatabase(workouts);

            const int Id = 2137;

            var response = await _httpClient.GetAsync($"api/workout/{Id}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task PostAsync_Success()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            var request = new AddWorkoutRequest
            {
                Description = "desc",
                Name = "name",
                UserId = userId,
            };

            var response = await _httpClient.PostAsJsonAsync("api/workout", request);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(GetFromDatabase<Workout>(), 
                workout => workout.Name == request.Name &&
                workout.UserId == userId &&
                workout.Description == request.Description);
        }

        [Fact]
        public async Task PostAsync_BadRequest()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            var request = new AddWorkoutRequest
            {
                Description = "desc",
            };

            var response = await _httpClient.PostAsJsonAsync("api/workout", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.DoesNotContain(GetFromDatabase<Workout>(),
                workout => workout.Name == request.Name &&
                workout.UserId == userId &&
                workout.Description == request.Description);
        }

        [Fact]
        public async Task PutAsync_Success()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            var workout = new Workout
            {
                Id = 1,
                UserId = userId,
                Description = "desc",
                Name = "name"
            };

            await AddToDatabase(new List<Workout> { workout });

            var request = new UpdateWorkoutRequest
            {
                Id = workout.Id,
                Description = "new desc",
                Name = "new name"
            };

            var response = await _httpClient.PutAsJsonAsync("api/workout", request);

            var compareWorkout = GetFromDatabase<Workout>().FirstOrDefault(x => x.Id == workout.Id);    

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Equal(compareWorkout.Name, request.Name);
            Assert.Equal(compareWorkout.Description, request.Description);
        }

        [Fact]
        public async Task PutAsync_BadRequest()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            var workout = new Workout
            {
                Id = 1,
                UserId = userId,
                Description = "desc",
                Name = "name"
            };

            await AddToDatabase(new List<Workout> { workout });

            var request = new UpdateWorkoutRequest
            {
                Description = "new desc",
                Name = "new name"
            };

            var response = await _httpClient.PutAsJsonAsync("api/workout", request);

            var compareWorkout = GetFromDatabase<Workout>().FirstOrDefault(x => x.Id == workout.Id);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.NotEqual(compareWorkout.Name, request.Name);
            Assert.NotEqual(compareWorkout.Description, request.Description);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            var workouts = new List<Workout>
            {
                new Workout { Id = 1, UserId = userId, Description = "desc1", Name = "name1" },
                new Workout { Id = 2, UserId = userId, Description = "desc2", Name = "name2" },
                new Workout { Id = 3, UserId = userId, Description = "desc3", Name = "name3" },
                new Workout { Id = 4, UserId = userId, Description = "desc4", Name = "name4" },
                new Workout { Id = 5, UserId = userId, Description = "desc5", Name = "name5" },
            };

            await AddToDatabase(workouts);

            const int Id = 3;

            var response = await _httpClient.DeleteAsync($"api/workout/{Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(GetFromDatabase<Workout>(), x => x.Id == Id);
        }

        [Fact]
        public async Task DeleteAsync_BadRequest()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            var workouts = new List<Workout>
            {
                new Workout { Id = 1, UserId = userId, Description = "desc1", Name = "name1" },
                new Workout { Id = 2, UserId = userId, Description = "desc2", Name = "name2" },
                new Workout { Id = 3, UserId = userId, Description = "desc3", Name = "name3" },
                new Workout { Id = 4, UserId = userId, Description = "desc4", Name = "name4" },
                new Workout { Id = 5, UserId = userId, Description = "desc5", Name = "name5" },
            };

            await AddToDatabase(workouts);

            const int Id = 2137;

            var response = await _httpClient.DeleteAsync($"api/workout/{Id}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
