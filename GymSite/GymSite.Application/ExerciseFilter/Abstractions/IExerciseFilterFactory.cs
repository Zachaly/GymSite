using GymSite.Domain.Entity;
using GymSite.Models.ExerciseFilter;
using GymSite.Models.ExerciseFilter.Request;

namespace GymSite.Application.Abstractions
{
    public interface IExerciseFilterFactory
    {
        ExerciseFilter Create(AddFilterRequest request);
        ExerciseFilterModel CreateModel(ExerciseFilter filter);
    }
}
