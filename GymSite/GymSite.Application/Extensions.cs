using FluentValidation.Results;

namespace GymSite.Application
{
    public static class Extensions
    {
        public static Dictionary<string, IEnumerable<string>> GetValidationErrors(this ValidationResult @this)
        {
            var dict = new Dictionary<string, IEnumerable<string>>();

            var properties = @this.Errors.Select(x => x.PropertyName).Distinct();

            foreach(var prop in properties)
            {
                var errors = @this.Errors.Where(x => x.PropertyName == prop).Select(x => x.ErrorMessage);
                dict.Add(prop, errors.ToList());
            }

            return dict;
        }
            
    }
}
