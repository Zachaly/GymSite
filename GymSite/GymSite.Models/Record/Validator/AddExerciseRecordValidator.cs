using FluentValidation;
using GymSite.Models.Record.Request;

namespace GymSite.Models.Record.Validator
{
    public class AddExerciseRecordValidator : AbstractValidator<AddExerciseRecordRequest>
    {
        public AddExerciseRecordValidator()
        {
            RuleFor(x => x.Reps).GreaterThan(0);
            RuleFor(x => x.ExerciseId).GreaterThan(0);
            RuleFor(x => x.Weight).GreaterThan(0);
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
