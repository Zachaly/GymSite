using FluentValidation;
using GymSite.Models.Workout.Requests;

namespace GymSite.Models.Workout.Validator
{
    public class AddWorkoutExerciseValidator : AbstractValidator<AddWorkoutExerciseRequest>
    {
        public AddWorkoutExerciseValidator()
        {
            RuleFor(x => x.ExerciseId).GreaterThan(0);
            RuleFor(x => x.WorkoutId).GreaterThan(0);
        }
    }
}
