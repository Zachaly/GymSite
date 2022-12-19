using GymSite.Domain.Entity;
using GymSite.Models.Record;
using GymSite.Models.Record.Request;
using GymSite.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GymSite.Tests.Integration
{
    public class ExerciseRecordControllerTests : IntegrationTest
    {
        [Fact]
        public async Task AddRecord_Success()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            var request = new AddExerciseRecordRequest
            {
                ExerciseId = 1,
                Reps = 1,
                UserId = userId,
                Weight = 1,
            };

            var response = await _httpClient.PostAsJsonAsync("api/exercise-record", request);
            var content = await response.Content.ReadFromJsonAsync<DataResponseModel<ExerciseRecordModel>>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains(GetFromDatabase<ExerciseRecord>(), x => x.UserId == userId);
        }

        [Fact]
        public async Task AddRecord_Fail_InvalidRequest()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            var request = new AddExerciseRecordRequest
            {
                ExerciseId = 1,
            };

            var response = await _httpClient.PostAsJsonAsync("api/exercise-record", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RemoveExerciseRecord_Success()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            var records = new List<ExerciseRecord>
            {
                new ExerciseRecord
                {
                    Id = 1,
                    Reps = 1,
                    Date = DateTime.UtcNow,
                    ExerciseId = 2,
                    Weight = 1,
                    UserId = userId
                },
                new ExerciseRecord
                {
                    Id = 2,
                    Reps = 1,
                    Date = DateTime.UtcNow,
                    ExerciseId = 2,
                    Weight = 1,
                    UserId = userId
                },
                new ExerciseRecord
                {
                    Id = 3,
                    Reps = 1,
                    Date = DateTime.UtcNow,
                    ExerciseId = 2,
                    Weight = 1,
                    UserId = userId
                },
            };

            await AddToDatabase(records);

            const int Id = 3;

            var response = await _httpClient.DeleteAsync($"api/exercise-record/{Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.DoesNotContain(GetFromDatabase<ExerciseRecord>(), x => x.Id == Id);
        }

        [Fact]
        public async Task RemoveExerciseRecord_Fail()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            const int Id = 6;

            var response = await _httpClient.DeleteAsync($"api/exercise-record/{Id}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
