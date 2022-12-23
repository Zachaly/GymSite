using GymSite.Domain.Entity;
using GymSite.Models.ExerciseFilter;
using GymSite.Models.ExerciseFilter.Request;
using GymSite.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace GymSite.Tests.Integration
{
    public class ExerciseFilterControllerTests : IntegrationTest
    {
        [Fact]
        public async Task Get()
        {
            var filters = new List<ExerciseFilter>
            {
                new ExerciseFilter { Id = 1, Name = "name1" },
                new ExerciseFilter { Id = 2, Name = "name2" },
                new ExerciseFilter { Id = 3, Name = "name3" },
                new ExerciseFilter { Id = 4, Name = "name4" },
            };

            await AddToDatabase(filters);

            var res = await _httpClient.GetAsync("api/exercise-filter");

            var content = await res.Content.ReadFromJsonAsync<DataResponseModel<IEnumerable<ExerciseFilterModel>>>();

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
            Assert.Equal(filters.Count, content.Data.Count());
        }

        [Fact]
        public async Task Post_Success()
        {
            await Authenticate();

            var request = new AddFilterRequest { Name = "test" };
            var res = await _httpClient.PostAsJsonAsync("api/exercise-filter", request);

            var content = await res.Content.ReadFromJsonAsync<DataResponseModel<ExerciseFilterModel>>();

            Assert.Equal(HttpStatusCode.OK, res.StatusCode);
            Assert.Equal(request.Name, content.Data.Name);
            Assert.Contains(GetFromDatabase<ExerciseFilter>(), x => x.Name == request.Name);
        }

        [Fact]
        public async Task Post_Fail()
        {
            await Authenticate();

            var request = new AddFilterRequest { };
            var res = await _httpClient.PostAsJsonAsync("api/exercise-filter", request);

            var content = await res.Content.ReadFromJsonAsync<DataResponseModel<ExerciseFilterModel>>();

            Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
            Assert.DoesNotContain(GetFromDatabase<ExerciseFilter>(), x => x.Name == request.Name);
        }

        [Fact]
        public async Task Delete_Success()
        {
            await Authenticate();

            var filters = new List<ExerciseFilter>
            {
                new ExerciseFilter { Id = 1, Name = "name1" },
                new ExerciseFilter { Id = 2, Name = "name2" },
                new ExerciseFilter { Id = 3, Name = "name3" },
                new ExerciseFilter { Id = 4, Name = "name4" },
            };

            await AddToDatabase(filters);

            const int Id = 3;

            var res = await _httpClient.DeleteAsync($"api/exercise-filter/{Id}");

            Assert.Equal(HttpStatusCode.NoContent, res.StatusCode);
            Assert.DoesNotContain(GetFromDatabase<ExerciseFilter>(), x => x.Id == Id);
        }

        [Fact]
        public async Task Delete_Fail()
        {
            await Authenticate();

            var filters = new List<ExerciseFilter>
            {
                new ExerciseFilter { Id = 1, Name = "name1" },
                new ExerciseFilter { Id = 2, Name = "name2" },
                new ExerciseFilter { Id = 3, Name = "name3" },
                new ExerciseFilter { Id = 4, Name = "name4" },
            };

            await AddToDatabase(filters);

            const int Id = 2137;

            var res = await _httpClient.DeleteAsync($"api/exercise-filter/{Id}");

            Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
        }
    }
}
