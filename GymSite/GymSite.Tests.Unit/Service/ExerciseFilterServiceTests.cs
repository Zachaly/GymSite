using GymSite.Application;
using GymSite.Application.Abstractions;
using GymSite.Database.Repository.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Models.ExerciseFilter;
using GymSite.Models.ExerciseFilter.Request;
using GymSite.Models.Response;
using Moq;

namespace GymSite.Tests.Unit.Service
{
    public class ExerciseFilterServiceTests
    {
        [Test]
        public async Task AddFilter_Success()
        {
            var filters = new List<ExerciseFilter>();

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<ExerciseFilterModel>(), ""))
                .Returns((ExerciseFilterModel data, string _) => new DataResponseModel<ExerciseFilterModel>
                {
                    Success = true,
                    Data = data
                });

            var exerciseFilterFactoryMock = new Mock<IExerciseFilterFactory>();
            exerciseFilterFactoryMock.Setup(x => x.Create(It.IsAny<AddFilterRequest>()))
                .Returns((AddFilterRequest request) => new ExerciseFilter { Name = request.Name });

            exerciseFilterFactoryMock.Setup(x => x.CreateModel(It.IsAny<ExerciseFilter>()))
                .Returns((ExerciseFilter filter) => new ExerciseFilterModel { Name = filter.Name });

            var repositoryMock = new Mock<IExerciseFilterRepository>();
            repositoryMock.Setup(x => x.AddFilter(It.IsAny<ExerciseFilter>()))
                .Callback((ExerciseFilter filter) => filters.Add(filter));

            var service = new ExerciseFilterService(repositoryMock.Object, responseFactoryMock.Object, exerciseFilterFactoryMock.Object);

            var request = new AddFilterRequest
            {
                Name = "name"
            };

            var res = await service.AddFilter(request);

            Assert.Multiple(() =>
            {
                Assert.That(filters.Any(x => x.Name == request.Name));
                Assert.That(res.Data.Name, Is.EqualTo(request.Name));
            });
        }

        [Test]
        public async Task AddFilter_Fail()
        {
            var filters = new List<ExerciseFilter>();

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateFail<ExerciseFilterModel>(It.IsAny<string>(), null))
                .Returns(new DataResponseModel<ExerciseFilterModel>
                {
                    Success = false,
                    Data = null
                });

            var exerciseFilterFactoryMock = new Mock<IExerciseFilterFactory>();
            exerciseFilterFactoryMock.Setup(x => x.Create(It.IsAny<AddFilterRequest>()))
                .Callback(() => throw new Exception());

            exerciseFilterFactoryMock.Setup(x => x.CreateModel(It.IsAny<ExerciseFilter>()))
                .Returns((ExerciseFilter filter) => new ExerciseFilterModel { Name = filter.Name });

            var repositoryMock = new Mock<IExerciseFilterRepository>();
            repositoryMock.Setup(x => x.AddFilter(It.IsAny<ExerciseFilter>()))
                .Callback((ExerciseFilter filter) => filters.Add(filter));

            var service = new ExerciseFilterService(repositoryMock.Object, responseFactoryMock.Object, exerciseFilterFactoryMock.Object);

            var request = new AddFilterRequest
            {
                Name = "name"
            };

            var res = await service.AddFilter(request);

            Assert.Multiple(() =>
            {
                Assert.That(!res.Success);
                Assert.That(res.Data, Is.Null);
            });
        }

        [Test]
        public async Task RemoveFilter_Success()
        {
            var filters = new List<ExerciseFilter>
            {
                new ExerciseFilter { Name = "name1", Id = 1 },
                new ExerciseFilter { Name = "name2", Id = 2 },
                new ExerciseFilter { Name = "name3", Id = 3 },
            };

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<string>()))
                .Returns(new ResponseModel
                {
                    Success = true,
                });

            var exerciseFilterFactoryMock = new Mock<IExerciseFilterFactory>();

            var repositoryMock = new Mock<IExerciseFilterRepository>();
            repositoryMock.Setup(x => x.RemoveFilter(It.IsAny<int>()))
                .Callback((int id) => filters.Remove(filters.FirstOrDefault(x => x.Id == id)));

            var service = new ExerciseFilterService(repositoryMock.Object, responseFactoryMock.Object, exerciseFilterFactoryMock.Object);
            const int Id = 2;

            var res = await service.RemoveFilter(Id);

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(!filters.Any(x => x.Id == Id));
            });
        }

        [Test]
        public async Task RemoveFilter_Fail()
        {
            var filters = new List<ExerciseFilter>
            {
                new ExerciseFilter { Name = "name1", Id = 1 },
                new ExerciseFilter { Name = "name2", Id = 2 },
                new ExerciseFilter { Name = "name3", Id = 3 },
            };

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateFail(It.IsAny<string>(), null))
                .Returns(new ResponseModel
                {
                    Success = false,
                });

            var exerciseFilterFactoryMock = new Mock<IExerciseFilterFactory>();
            exerciseFilterFactoryMock.Setup(x => x.CreateModel(It.IsAny<ExerciseFilter>()))
                .Returns((ExerciseFilter filter) => new ExerciseFilterModel { Name = filter.Name });

            var repositoryMock = new Mock<IExerciseFilterRepository>();
            repositoryMock.Setup(x => x.RemoveFilter(It.IsAny<int>()))
                .Callback((int id) => throw new Exception());

            var service = new ExerciseFilterService(repositoryMock.Object, responseFactoryMock.Object, exerciseFilterFactoryMock.Object);
            const int Id = 2;

            var res = await service.RemoveFilter(Id);

            Assert.Multiple(() =>
            {
                Assert.That(!res.Success);
                Assert.That(filters.Any(x => x.Id == Id));
            });
        }

        [Test]
        public async Task GetFilters()
        {
            var filters = new List<ExerciseFilter>
            {
                new ExerciseFilter { Name = "name1", Id = 1 },
                new ExerciseFilter { Name = "name2", Id = 2 },
                new ExerciseFilter { Name = "name3", Id = 3 },
            };

            var responseFactoryMock = new Mock<IResponseFactory>();
            responseFactoryMock.Setup(x => x.CreateSuccess(It.IsAny<IEnumerable<ExerciseFilterModel>>(), ""))
                .Returns((IEnumerable<ExerciseFilterModel> data, string _) => new DataResponseModel<IEnumerable<ExerciseFilterModel>>
                {
                    Success = true,
                    Data = data
                });

            var exerciseFilterFactoryMock = new Mock<IExerciseFilterFactory>();

            var repositoryMock = new Mock<IExerciseFilterRepository>();
            repositoryMock.Setup(x => x.GetFilters(It.IsAny<Func<ExerciseFilter, ExerciseFilterModel>>()))
                .Returns((Func<ExerciseFilter, ExerciseFilterModel> selector)
                    => filters.Select(selector));

            var service = new ExerciseFilterService(repositoryMock.Object, responseFactoryMock.Object, exerciseFilterFactoryMock.Object);

            var res = service.GetFilters();

            Assert.Multiple(() =>
            {
                Assert.That(res.Success);
                Assert.That(res.Data.Count(), Is.EqualTo(filters.Count));
            });
        }
    }
}
