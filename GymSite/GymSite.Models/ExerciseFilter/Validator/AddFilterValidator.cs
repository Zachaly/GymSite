using FluentValidation;
using GymSite.Models.ExerciseFilter.Request;

namespace GymSite.Models.ExerciseFilter.Validator
{
    public class AddFilterValidator : AbstractValidator<AddFilterRequest>
    {
        public AddFilterValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(20);
        }
    }
}
