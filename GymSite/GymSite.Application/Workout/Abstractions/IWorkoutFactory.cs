using GymSite.Domain.Entity;
using GymSite.Models.Workout;
using GymSite.Models.Workout.Requests;

namespace GymSite.Application.Abstractions
{
    public interface IWorkoutFactory
    {
        WorkoutModel CreateModel(Workout workout);
        Workout Create(AddWorkoutRequest request);
        WorkoutListItemModel CreateListItem(Workout workout);
    }
}
