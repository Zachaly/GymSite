using FluentValidation;
using GymSite.Models.Exercise.Request;

namespace GymSite.Models.Exercise.Validator
{
    public class AddExerciseValidator : AbstractValidator<AddExerciseRequest>
    {
        public AddExerciseValidator()
        {
            RuleFor(x => x.Description).MaximumLength(200);
            RuleFor(x => x.Name).NotEmpty().MinimumLength(5).MaximumLength(100);
            RuleFor(x => x.FilterIds).Must(x => x?.All(y => y > 0) ?? true);
        }
    }
}
