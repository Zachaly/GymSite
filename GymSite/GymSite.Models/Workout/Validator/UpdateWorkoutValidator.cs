using FluentValidation;
using GymSite.Models.Workout.Requests;

namespace GymSite.Models.Workout.Validator
{
    public class UpdateWorkoutValidator : AbstractValidator<UpdateWorkoutRequest>
    {
        public UpdateWorkoutValidator()
        {
            RuleFor(x => x.Name).MinimumLength(3).MaximumLength(30);
            RuleFor(x => x.Description).MaximumLength(200);
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}
