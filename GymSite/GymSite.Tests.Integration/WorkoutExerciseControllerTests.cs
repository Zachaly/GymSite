using GymSite.Domain.Entity;
using GymSite.Models.Response;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GymSite.Tests.Integration
{
    public class WorkoutExerciseControllerTests : IntegrationTest
    {
        [Fact]
        public async Task PostAsync_Success()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            var exercise = new Exercise { Id = 1, UserId = userId, Name = "name", Description = "desc" };

            await AddToDatabase(new List<Exercise> { exercise });

            var request = new AddWorkoutExerciseRequest
            {
                ExerciseId = exercise.Id,
                WorkoutId = 2137,
            };

            var response = await _httpClient.PostAsJsonAsync("api/workout-exercise", request);

            var content = await response.Content.ReadFromJsonAsync<DataResponseModel<WorkoutExerciseModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(content.Data.ExerciseName, exercise.Name);
            Assert.Equal(content.Data.ExerciseDescription, exercise.Description);
            Assert.Equal(content.Data.ExerciseId, request.ExerciseId);
            Assert.Contains(GetFromDatabase<WorkoutExercise>(), 
                x => x.ExerciseId == request.ExerciseId &&
                x.WorkoutId == request.WorkoutId);
        }

        [Fact]
        public async Task PostAsync_BadRequest()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            var exercise = new Exercise { Id = 1, UserId = userId, Name = "name", Description = "desc" };

            await AddToDatabase(new List<Exercise> { exercise });

            var request = new AddWorkoutExerciseRequest();

            var response = await _httpClient.PostAsJsonAsync("api/workout-exercise", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.DoesNotContain(GetFromDatabase<WorkoutExercise>(),
                x => x.ExerciseId == request.ExerciseId &&
                x.WorkoutId == request.WorkoutId);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            var exercises = new List<WorkoutExercise>
            {
                new WorkoutExercise { Id = 1, ExerciseId = 21, WorkoutId = 37 },
                new WorkoutExercise { Id = 2, ExerciseId = 21, WorkoutId = 37 },
                new WorkoutExercise { Id = 3, ExerciseId = 21, WorkoutId = 37 },
                new WorkoutExercise { Id = 4, ExerciseId = 21, WorkoutId = 37 },
                new WorkoutExercise { Id = 5, ExerciseId = 21, WorkoutId = 37 },
            };

            await AddToDatabase(exercises);

            const int Id = 3;

            var response = await _httpClient.DeleteAsync($"api/workout-exercise/{Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(GetFromDatabase<WorkoutExercise>(), x => x.Id == Id);
        }

        [Fact]
        public async Task DeleteAsync_BadRequest()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            var exercises = new List<WorkoutExercise>
            {
                new WorkoutExercise { Id = 1, ExerciseId = 21, WorkoutId = 37 },
                new WorkoutExercise { Id = 2, ExerciseId = 21, WorkoutId = 37 },
                new WorkoutExercise { Id = 3, ExerciseId = 21, WorkoutId = 37 },
                new WorkoutExercise { Id = 4, ExerciseId = 21, WorkoutId = 37 },
                new WorkoutExercise { Id = 5, ExerciseId = 21, WorkoutId = 37 },
            };

            await AddToDatabase(exercises);

            const int Id = 2137;

            var response = await _httpClient.DeleteAsync($"api/workout-exercise/{Id}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
