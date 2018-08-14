using System.Text.RegularExpressions;

namespace Mvc.Framework.Attributes.Validation
{
    public class RegexAttribute : PropertyValidationAttribute
    {
        private readonly string pattern;

        public RegexAttribute(string pattern)
        {
            this.pattern = $"^{pattern}$";
        }

        public override bool IsValid(object value)
        {
            if (!(value is string valueAsString))
            {
                return true;
            }

            return Regex.IsMatch(valueAsString, this.pattern);
        }
    }
}
