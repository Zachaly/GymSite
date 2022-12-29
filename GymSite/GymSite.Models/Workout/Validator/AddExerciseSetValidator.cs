using FluentValidation;
using GymSite.Models.Workout.Requests;

namespace GymSite.Models.Workout.Validator
{
    public class AddExerciseSetValidator : AbstractValidator<AddExerciseSetRequest>
    {
        public AddExerciseSetValidator()
        {
            RuleFor(x => x.Reps).GreaterThan(0);
            RuleFor(x => x.WorkoutExerciseId).GreaterThan(0);
            RuleFor(x => x.Weight).GreaterThan(0);
        }
    }
}
