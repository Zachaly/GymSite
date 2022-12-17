using GymSite.Domain.Entity;
using GymSite.Models.Exercise;
using GymSite.Models.Exercise.Request;

namespace GymSite.Application.Abstractions
{
    public interface IExerciseFactory
    {
        Exercise Create(AddExerciseRequest request);
        ExerciseModel CreateModel(Exercise exercise);
        ExerciseListItemModel CreateListItem(Exercise exercise);
        Exercise CreateDefault(AddExerciseRequest request);
    }
}