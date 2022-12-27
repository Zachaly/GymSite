using GymSite.Domain.Entity;
using GymSite.Models.Response;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;
using System.Net.Http.Json;

namespace GymSite.Tests.Integration
{
    public class ExerciseSetControllerTests : IntegrationTest
    {
        [Fact]
        public async Task PostAsync_Success()
        {
            await Authenticate();

            var request = new AddExerciseSetRequest
            {
                Reps = 1,
                Weight = 2,
                WorkoutExerciseId = 3
            };

            var response = await _httpClient.PostAsJsonAsync("api/exercise-set", request);

            var content = await response.Content.ReadFromJsonAsync<DataResponseModel<ExerciseSetModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(GetFromDatabase<ExerciseSet>(),
                x => x.Reps == request.Reps &&
                x.ExerciseId == request.WorkoutExerciseId &&
                x.Weigth == request.Weight);
            Assert.Equal(content.Data.Weight, request.Weight.ToString());
            Assert.Equal(content.Data.Reps, request.Reps);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            await Authenticate();

            var sets = new List<ExerciseSet>
            {
                new ExerciseSet { Id = 1, ExerciseId = 2, Reps = 3, Weigth = 4 },
                new ExerciseSet { Id = 2, ExerciseId = 2, Reps = 3, Weigth = 4 },
                new ExerciseSet { Id = 3, ExerciseId = 2, Reps = 3, Weigth = 4 },
                new ExerciseSet { Id = 4, ExerciseId = 2, Reps = 3, Weigth = 4 },
            };

            await AddToDatabase(sets);

            const int Id = 3;

            var response = await _httpClient.DeleteAsync($"api/exercise-set/{Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(GetFromDatabase<ExerciseSet>(), x => x.Id == Id);
        }

        [Fact]
        public async Task DeleteAsync_BadRequest()
        {
            await Authenticate();

            var sets = new List<ExerciseSet>
            {
                new ExerciseSet { Id = 1, ExerciseId = 2, Reps = 3, Weigth = 4 },
                new ExerciseSet { Id = 2, ExerciseId = 2, Reps = 3, Weigth = 4 },
                new ExerciseSet { Id = 3, ExerciseId = 2, Reps = 3, Weigth = 4 },
                new ExerciseSet { Id = 4, ExerciseId = 2, Reps = 3, Weigth = 4 },
            };

            await AddToDatabase(sets);

            const int Id = 2137;

            var response = await _httpClient.DeleteAsync($"api/exercise-set/{Id}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
