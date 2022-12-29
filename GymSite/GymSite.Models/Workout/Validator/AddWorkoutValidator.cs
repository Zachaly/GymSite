using FluentValidation;
using GymSite.Models.Workout.Requests;

namespace GymSite.Models.Workout.Validator
{
    public class AddWorkoutValidator : AbstractValidator<AddWorkoutRequest>
    {
        public AddWorkoutValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MinimumLength(3).MaximumLength(30);
            RuleFor(x => x.Description).MaximumLength(200);
        }
    }
}
