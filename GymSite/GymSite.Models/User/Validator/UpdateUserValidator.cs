using FluentValidation;
using GymSite.Models.User.Request;

namespace GymSite.Models.User.Validator
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.NickName).MinimumLength(5);
            RuleFor(x => x.FirstName).MaximumLength(50);
            RuleFor(x => x.LastName).MaximumLength(50);
        }
    }
}
