using FluentValidation;
using GymSite.Models.Article.Request;
namespace GymSite.Models.Article.Validator
{
    public class AddArticleValidator : AbstractValidator<AddArticleRequest>
    {
        public AddArticleValidator()
        {
            RuleFor(x => x.Title).NotEmpty().MinimumLength(5).MaximumLength(50);
            RuleFor(x => x.Description).MaximumLength(100);
            RuleFor(x => x.Content).NotEmpty().MinimumLength(100).MaximumLength(1500);
            RuleFor(x => x.CreatorId).NotEmpty();
        }
    }
}
