using GymSite.Application.Abstractions;
using GymSite.Domain.Entity;
using GymSite.Domain.Utils;
using GymSite.Models.ExerciseFilter;
using GymSite.Models.ExerciseFilter.Request;

namespace GymSite.Application
{
    [Implementation(typeof(IExerciseFilterFactory))]
    public class ExerciseFilterFactory : IExerciseFilterFactory
    {
        public ExerciseFilter Create(AddFilterRequest request)
            => new ExerciseFilter
            {
                Name = request.Name,
            };

        public ExerciseFilterModel CreateModel(ExerciseFilter filter)
            => new ExerciseFilterModel
            {
                Name = filter.Name,
                Id = filter.Id,
            };
    }
}
