using GymSite.Domain.Entity;
using GymSite.Models.Exercise;
using GymSite.Models.Exercise.Request;
using GymSite.Models.Response;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace GymSite.Tests.Integration
{
    public class ExerciseControllerTests : IntegrationTest
    {
        [Fact]
        public async Task GetUserExercices_Success()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            var exercises = new List<Exercise>
            {
                new Exercise
                {
                    Default = true,
                    Name = "ex1",
                    UserId = userId,
                },
                new Exercise
                {
                    Name = "ex2",
                    UserId = userId,
                },
                new Exercise
                {
                    Name = "ex3",
                    UserId = "id",
                },
            };

            await AddToDatabase(exercises);

            var response = await _httpClient.GetAsync($"api/exercise/user/${userId}");

            var content = await response.Content.ReadFromJsonAsync<DataResponseModel<IEnumerable<ExerciseListItemModel>>>();

            var test = exercises.Where(x => x.Default || x.UserId == userId).ToList();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(content.Data);
            Assert.All(content.Data, x => test.Any(y => y.Id == x.Id));
        }

        [Fact]
        public async Task GetExerciseById_Success()
        {
            await Authenticate();

            var exercises = new List<Exercise>
            {
                new Exercise
                {
                    Id = 1,
                    Default = true,
                    Name = "ex1",
                    UserId = "id3",
                },
                new Exercise
                {
                    Id = 2,
                    Name = "ex2",
                    UserId = "id2",
                },
                new Exercise
                {
                    Id = 3,
                    Name = "ex3",
                    UserId = "id",
                },
            };

            await AddToDatabase(exercises);

            const int Id = 3;

            var response = await _httpClient.GetAsync($"api/exercise/{Id}");

            var content = await response.Content.ReadFromJsonAsync<DataResponseModel<ExerciseModel>>();

            var testExercise = exercises.FirstOrDefault(x => x.Id == Id);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(testExercise.Name, content.Data.Name);
        }

        [Fact]
        public async Task GetExerciseById_Fail()
        {
            await Authenticate();

            var exercises = new List<Exercise>
            {
                new Exercise
                {
                    Id = 1,
                    Default = true,
                    Name = "ex1",
                    UserId = "id3",
                },
                new Exercise
                {
                    Id = 2,
                    Name = "ex2",
                    UserId = "id2",
                },
                new Exercise
                {
                    Id = 3,
                    Name = "ex3",
                    UserId = "id",
                },
            };

            await AddToDatabase(exercises);

            const int Id = 4;

            var response = await _httpClient.GetAsync($"api/exercise/{Id}");

            var content = await response.Content.ReadFromJsonAsync<DataResponseModel<ExerciseModel>>();

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Null(content.Data);
        }

        [Fact]
        public async Task AddExercise_Success()
        {
            await Authenticate();

            var request = new AddExerciseRequest
            {
                Description = "desc",
                Name = "ex name",
                UserId = GetAuthenticatedUser().Id,
                FilterIds = new List<int>()
            };

            var response = await _httpClient.PostAsJsonAsync("api/exercise", request);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(GetFromDatabase<Exercise>(), x => x.Name== request.Name);
        }

        [Fact]
        public async Task AddExercise_Fail()
        {
            await Authenticate();

            var request = new AddExerciseRequest();

            var response = await _httpClient.PostAsJsonAsync("api/exercise", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task RemoveExercise_Success()
        {
            await Authenticate();

            var exercises = new List<Exercise>
            {
                new Exercise
                {
                    Id = 1,
                    Default = true,
                    Name = "ex1",
                    UserId = "id3",
                },
                new Exercise
                {
                    Id = 2,
                    Name = "ex2",
                    UserId = "id2",
                },
                new Exercise
                {
                    Id = 3,
                    Name = "ex3",
                    UserId = "id",
                },
            };

            await AddToDatabase(exercises);

            const int Id = 3;

            var response = await _httpClient.DeleteAsync($"api/exercise/{Id}");

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task RemoveExercise_Fail()
        {
            await Authenticate();

            var exercises = new List<Exercise>
            {
                new Exercise
                {
                    Id = 1,
                    Default = true,
                    Name = "ex1",
                    UserId = "id3",
                },
                new Exercise
                {
                    Id = 2,
                    Name = "ex2",
                    UserId = "id2",
                },
                new Exercise
                {
                    Id = 3,
                    Name = "ex3",
                    UserId = "id",
                },
            };

            await AddToDatabase(exercises);

            const int Id = 10;

            var response = await _httpClient.DeleteAsync($"api/exercise/{Id}");

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetDefaultExercises_Success()
        {
            await Authenticate();

            var exercises = new List<Exercise>
            {
                new Exercise
                {
                    Id = 1,
                    Default = true,
                    Name = "ex1",
                    UserId = "id3",
                },
                new Exercise
                {
                    Default = false,
                    Id = 2,
                    Name = "ex2",
                    UserId = "id2",
                },
                new Exercise
                {
                    Default = true,
                    Id = 3,
                    Name = "ex3",
                    UserId = "id",
                },
            };

            await AddToDatabase(exercises);

            var response = await _httpClient.GetAsync($"api/exercise/default");

            var content = await response.Content.ReadFromJsonAsync<DataResponseModel<IEnumerable<ExerciseListItemModel>>>();

            var testCount = exercises.Where(x => x.Default).Count();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(testCount, content.Data.Count());
        }

        [Fact]
        public async Task AddDefaultExercise_Success()
        {
            await Authenticate();

            var request = new AddExerciseRequest
            {
                Description = "desc",
                Name = "ex name",
                UserId = "id",
                FilterIds = new List<int>()
            };

            var response = await _httpClient.PostAsJsonAsync($"api/exercise/default", request);

            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.Contains(GetFromDatabase<Exercise>(), x => x.Default && x.Name == request.Name);
        }

        [Fact]
        public async Task AddDefaultExercise_Fail_InvalidRequest()
        {
            await Authenticate();

            var request = new AddExerciseRequest
            {
                UserId = "id",
            };

            var response = await _httpClient.PostAsJsonAsync($"api/exercise/default", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task GetExercisesWithFilter_Success()
        {
            await Authenticate();

            var userId = GetAuthenticatedUser().Id;

            var exercises = new List<Exercise>
            {
                new Exercise
                {
                    Default = true,
                    Name = "ex1",
                    UserId = userId,
                    ExerciseFilters = new List<ExerciseExerciseFilter>
                    {
                        new ExerciseExerciseFilter { FilterId = 11 },
                        new ExerciseExerciseFilter { FilterId = 12 },
                        new ExerciseExerciseFilter { FilterId = 13 },
                    }
                },
                new Exercise
                {
                    Name = "ex2",
                    UserId = userId,
                    ExerciseFilters = new List<ExerciseExerciseFilter>
                    {
                        new ExerciseExerciseFilter { FilterId = 10 },
                        new ExerciseExerciseFilter { FilterId = 14 },
                        new ExerciseExerciseFilter { FilterId = 15 },
                    }
                },
                new Exercise
                {
                    Name = "ex3",
                    UserId = userId,
                    ExerciseFilters = new List<ExerciseExerciseFilter>
                    {
                        new ExerciseExerciseFilter { FilterId = 10 },
                        new ExerciseExerciseFilter { FilterId = 16 },
                        new ExerciseExerciseFilter { FilterId = 17 },
                    }
                },
            };

            await AddToDatabase(exercises);

            var request = new GetExerciseRequest
            {
                UserId = userId,
                FilterIds = new int[] { 10 }
            };

            var uri = QueryHelpers.AddQueryString("api/exercise/filter", new Dictionary<string, string>()
            {
                ["userId"] = request.UserId,
                ["filterIds"] = request.FilterIds.ElementAt(0).ToString(),
            });

            var res = await _httpClient.GetAsync(uri);

            var content = await res.Content.ReadFromJsonAsync<DataResponseModel<IEnumerable<ExerciseListItemModel>>>();

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
            Assert.Equal(2, content.Data.Count());
        }
    }
}
